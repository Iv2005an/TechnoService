using CommunityToolkit.Mvvm.ComponentModel;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class StatisticsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _currentUser;
}
