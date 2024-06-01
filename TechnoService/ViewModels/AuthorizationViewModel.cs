using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class AuthorizationViewModel : ObservableObject
{
    [ObservableProperty]
    private bool error = false;

    [RelayCommand]
    private void Register(UserModel user)
    {
    }
    [RelayCommand]
    private void Login(UserModel user)
    {
    }
}
