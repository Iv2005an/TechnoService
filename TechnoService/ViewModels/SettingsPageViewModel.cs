using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _currentUser;
    [ObservableProperty]
    private string _newPassword;
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
    private async Task UpdateUser() => 
        await UsersService.UpdateUser(CurrentUser);
}
