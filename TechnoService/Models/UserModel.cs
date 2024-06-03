using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;
using TechnoService.Services;

namespace TechnoService.Models;

public sealed partial class UserModel : ObservableObject
{
    public UserModel() { }
    public UserModel(IDataRecord record)
    {
        Id = record.GetInt32(0);
        Type = (UserTypes)Enum.GetValues(typeof(UserTypes)).GetValue(record.GetByte(1));
        Surname = record.GetString(2);
        Name = record.GetString(3);
        Patronymic = record.GetString(4);
        Login = record.GetString(5);
        Password.Hash = record.GetString(6);
        Password.Salt = record.GetString(7);
    }

    [ObservableProperty]
    private int _id;
    [ObservableProperty]
    private UserTypes _type = UserTypes.Client;
    [ObservableProperty]
    private string _surname;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _patronymic;
    [ObservableProperty]
    private string _login;
    [ObservableProperty]
    private Password _password = new();
}
