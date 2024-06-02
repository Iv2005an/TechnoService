using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class AuthorizationViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _currentUser = new();
    [ObservableProperty]
    private string _commandMessage;

    [RelayCommand]
    private async Task IsLoginFree()
    {
        var isLoginFree = await UsersService.IsLoginFree(CurrentUser.Login);
        if (isLoginFree) CommandMessage = null;
        else CommandMessage = "Логин занят\n";
    }
    [RelayCommand]
    private async Task Register()
    {
        CommandMessage = await UsersService.Register(CurrentUser);
        if (CommandMessage is null) CurrentUser = new();
    }
    [RelayCommand]
    private async Task Login()
    {
        var user = await UsersService.Login(CurrentUser);
        if (user is null) CommandMessage = "Логин или пароль не совпадают\n";
        else
        {
            CommandMessage = null;
            CurrentUser = user;
        }
    }
}
