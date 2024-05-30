using CommunityToolkit.Mvvm.ComponentModel;

namespace TechnoService.Models;

public sealed partial class UserModel : ObservableObject
{
    public enum UserTypes
    {
        Client = 0,
        Executor = 1,
        Admin = 2
    }

    [ObservableProperty]
    private string _login;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private UserTypes _type = UserTypes.Client;
}
