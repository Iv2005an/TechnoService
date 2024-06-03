using CommunityToolkit.Mvvm.ComponentModel;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class RequestsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _currentUser;
}
