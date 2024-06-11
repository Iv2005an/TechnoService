using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class EditUserPageViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _user;
    [RelayCommand]
    private async Task UpdateUser() =>
        await UsersService.UpdateUser(User);
}
