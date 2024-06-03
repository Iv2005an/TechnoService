using CommunityToolkit.Mvvm.ComponentModel;
using TechnoService.Services;

namespace TechnoService.Models;

public sealed partial class UserModel : ObservableObject
{
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
