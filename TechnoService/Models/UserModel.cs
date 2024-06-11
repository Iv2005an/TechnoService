using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Data;

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
    private PasswordModel _password = new();

    public string FullName => $"{Surname} {Name} {Patronymic}";
    public string NameWithInitials => $"{Surname} {Name[0]}. {Patronymic[0]}.";
    public string[] TypesNames => [
            "Клиент",
            "Исполнитель",
            "Админ" ];
    public int TypeIndex
    {
        get
        {
            return Convert.ToInt32(Type);
        }
        set
        {
            Type = (UserTypes)Enum.GetValues(typeof(UserTypes)).GetValue(value);
        }

    }

    public string TypeName => TypesNames[TypeIndex];

    public bool IsSuitable(string searchText) =>
        Id.ToString().Contains(searchText) ||
        TypeName.Contains(searchText) ||
        FullName.Contains(searchText) ||
        Login.Contains(searchText);
    public override string ToString()
    {
        return FullName;
    }
}
