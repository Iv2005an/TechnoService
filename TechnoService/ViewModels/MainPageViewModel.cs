using CommunityToolkit.Mvvm.ComponentModel;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    UserModel _currentUser;
}
