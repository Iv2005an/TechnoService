﻿using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using TechnoService.Models;

namespace TechnoService.Services;

public static class UsersService
{
    private static async Task Init()
    {
        using SqlConnection connection = Database.Connection;
        using SqlCommand createUsersTableCommand = new(
            "IF OBJECT_ID('users', 'U') IS NULL " +
            "CREATE TABLE users(" +
            "id int IDENTITY(1, 1) NOT NULL PRIMARY KEY, " +
            "type tinyint NOT NULL DEFAULT 0, " +
            "surname nvarchar(50) NOT NULL, " +
            "name nvarchar(50) NOT NULL, " +
            "patronymic nvarchar(50) NOT NULL,  " +
            "login nvarchar(50) UNIQUE NOT NULL, " +
            "password nvarchar(128) NOT NULL, " +
            "salt nvarchar(128) NOT NULL);", connection);
        await createUsersTableCommand.ExecuteNonQueryAsync();
    }
    public static async Task<bool> IsLoginFree(string login)
    {
        await Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand loginCountCommand = new($"SELECT COUNT(1) FROM users WHERE login=N'{login}';", connection);
        using SqlDataReader reader = await loginCountCommand.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return reader.GetInt32(0) == 0;
        return true;
    }
    public static async Task<string> Register(UserModel user)
    {
        await Init();
        if (!await IsLoginFree(user.Login)) return "Логин занят";
        user.Password.ComputeHash(null);
        using SqlConnection connection = Database.Connection;
        using SqlCommand registerCommand = new($"INSERT INTO USERS VALUES(" +
            $"0," +
            $"N'{user.Surname}'," +
            $"N'{user.Name}'," +
            $"N'{user.Patronymic ?? ""}'," +
            $"N'{user.Login}'," +
            $"N'{user.Password.Hash}'," +
            $"N'{user.Password.Salt}'" +
            $");", connection);
        await registerCommand.ExecuteNonQueryAsync();
        return null;
    }
    public static async Task<UserModel> Login(UserModel user)
    {
        await Init();
        if (await IsLoginFree(user.Login)) return null;
        using SqlConnection connection = Database.Connection;
        using SqlCommand getSaltCommand = new(
            $"SELECT salt " +
            $"FROM users " +
            $"WHERE login=N'{user.Login}'", connection);
        using (SqlDataReader saltReader = await getSaltCommand.ExecuteReaderAsync())
        {
            if (await saltReader.ReadAsync())
                user.Password.ComputeHash(saltReader.GetString(0));
            else return null;
        }
        using SqlCommand getUserCommand = new(
            $"SELECT * " +
            $"FROM users " +
            $"WHERE login=N'{user.Login}' AND " +
            $"password=N'{user.Password.Hash}'",
            connection);
        using SqlDataReader userReader = await getUserCommand.ExecuteReaderAsync();
        if (await userReader.ReadAsync())
        {
            if (user.Password.Hash == userReader.GetString(6))
            {
                user.Type = (UserTypes)Enum.GetValues(typeof(UserTypes)).GetValue(userReader.GetByte(1));
                user.Surname = userReader.GetString(2);
                user.Name = userReader.GetString(3);
                user.Patronymic = userReader.GetString(4);
                return user;
            };
        }
        return null;
    }
}
