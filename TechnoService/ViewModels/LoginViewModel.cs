using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class LoginViewModel:ObservableObject
{
    [RelayCommand]
    private void Logining(UserModel user)
    {
    }
}
