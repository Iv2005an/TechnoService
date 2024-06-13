using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnoService.Models;

namespace TechnoService.Services;

public static class UsersService
{
    private static void Init()
    {
        using SqlConnection connection = Database.Connection;
        using SqlCommand createUsersTableCommand = new(
            "IF OBJECT_ID('users', 'U') IS NULL " +
            "CREATE TABLE users(" +
            "id int IDENTITY(0, 1) NOT NULL PRIMARY KEY," +
            "type tinyint NOT NULL DEFAULT 0," +
            "surname nvarchar(50) NOT NULL," +
            "name nvarchar(50) NOT NULL," +
            "patronymic nvarchar(50) NOT NULL," +
            "login nvarchar(50) UNIQUE NOT NULL," +
            "password nvarchar(128) NOT NULL," +
            "salt nvarchar(128) NOT NULL);", connection);
        createUsersTableCommand.ExecuteNonQuery();

        SqlCommand loginCountCommand = new($"SELECT COUNT(1) FROM users WHERE login=N'admin';", connection);
        bool isAdminNotCreated = true;
        using (SqlDataReader reader = loginCountCommand.ExecuteReader())
        {
            if (reader.Read()) isAdminNotCreated = reader.GetInt32(0) == 0;
        }
        if (isAdminNotCreated)
        {
            PasswordModel password = new() { PasswordString = "Admin_1234" };
            password.ComputeHash();
            SqlCommand addAdminCommand = new(
            "INSERT INTO users VALUES(" +
            $"{Convert.ToInt32(UserTypes.Admin)}," +
            "N'Фамилия'," +
            "N'Имя'," +
            "N'Отчество'," +
            "N'admin'," +
            $"N'{password.Hash}'," +
            $"N'{password.Salt}');",
            connection);
            addAdminCommand.ExecuteNonQuery();
#if DEBUG
            password = new() { PasswordString = "Executor_1234" };
            password.ComputeHash();
            SqlCommand addExecutorCommand = new(
            "INSERT INTO users VALUES(" +
            $"{Convert.ToInt32(UserTypes.Executor)}," +
            "N'Фамилия'," +
            "N'Имя'," +
            "N'Отчество'," +
            "N'executor'," +
            $"N'{password.Hash}'," +
            $"N'{password.Salt}');",
            connection);
            addExecutorCommand.ExecuteNonQuery();
            password = new() { PasswordString = "Client_1234" };
            password.ComputeHash();
            SqlCommand addClientCommand = new(
            "INSERT INTO users VALUES(" +
            $"{Convert.ToInt32(UserTypes.Client)}," +
            "N'Фамилия'," +
            "N'Имя'," +
            "N'Отчество'," +
            "N'client'," +
            $"N'{password.Hash}'," +
            $"N'{password.Salt}');",
            connection);
            addClientCommand.ExecuteNonQuery();
#endif
        }
    }
    public static async Task<bool> IsLoginFree(string login)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand loginCountCommand = new($"SELECT COUNT(1) FROM users WHERE login=N'{login}';", connection);
        using SqlDataReader reader = await loginCountCommand.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return reader.GetInt32(0) == 0;
        return true;
    }
    public static async Task<string> Register(UserModel user)
    {
        Init();
        if (!await IsLoginFree(user.Login)) return "Логин занят";
        user.Password.ComputeHash();
        using SqlConnection connection = Database.Connection;
        using SqlCommand registerCommand = new(
            "INSERT INTO USERS VALUES(" +
            "0," +
            $"N'{user.Surname}'," +
            $"N'{user.Name}'," +
            $"N'{user.Patronymic ?? ""}'," +
            $"N'{user.Login}'," +
            $"N'{user.Password.Hash}'," +
            $"N'{user.Password.Salt}'" +
            ");", connection);
        await registerCommand.ExecuteNonQueryAsync();
        return null;
    }
    public static async Task<UserModel> Login(UserModel user)
    {
        Init();
        if (await IsLoginFree(user.Login)) return null;
        using SqlConnection connection = Database.Connection;
        using SqlCommand getSaltCommand = new(
            $"SELECT salt " +
            $"FROM users " +
            $"WHERE login=N'{user.Login}';", connection);
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
            $"password=N'{user.Password.Hash}';",
            connection);
        using SqlDataReader userReader = await getUserCommand.ExecuteReaderAsync();
        if (await userReader.ReadAsync())
        {
            user.Id = userReader.GetInt32(0);
            user.Type = (UserTypes)Enum.GetValues(typeof(UserTypes)).GetValue(userReader.GetByte(1));
            user.Surname = userReader.GetString(2);
            user.Name = userReader.GetString(3);
            user.Patronymic = userReader.GetString(4);
            return user;
        }
        return null;
    }
    public static UserModel GetUser(int id)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getUserCommand = new(
            "SELECT * " +
            "FROM users " +
            $"WHERE id={id};",
            connection);
        using SqlDataReader reader = getUserCommand.ExecuteReader();
        return reader.Read() ? new UserModel(reader) : null;
    }
    public static List<UserModel> GetUsers()
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getUsersCommand = new("SELECT * FROM users;", connection);
        using SqlDataReader reader = getUsersCommand.ExecuteReader();
        List<UserModel> users = [];
        while (reader.Read()) users.Add(new(reader));
        return users;
    }
    public static List<UserModel> GetExecutors()
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand getExecutorsCommand = new(
            "SELECT * " +
            "FROM users " +
            $"WHERE type={Convert.ToInt32(UserTypes.Admin)}" +
            $"OR type={Convert.ToInt32(UserTypes.Executor)};",
            connection);
        using SqlDataReader reader = getExecutorsCommand.ExecuteReader();
        List<UserModel> executors = [];
        while (reader.Read()) executors.Add(new(reader));
        return executors;
    }
    public static async Task UpdateUser(UserModel user)
    {
        Init();
        using SqlConnection connection = Database.Connection;
        using SqlCommand updateRequestCommand = new(
            "UPDATE users SET " +
            $"type='{user.TypeIndex}'," +
            $"surname=N'{user.Surname}'," +
            $"name=N'{user.Name}'," +
            $"patronymic=N'{user.Patronymic}'," +
            $"password=N'{user.Password.Hash}'," +
            $"salt=N'{user.Password.Salt}' " +
            $"WHERE id={user.Id};",
            connection);
        await updateRequestCommand.ExecuteNonQueryAsync();
    }
}
