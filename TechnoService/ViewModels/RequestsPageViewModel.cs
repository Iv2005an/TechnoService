using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TechnoService.Models;

namespace TechnoService.ViewModels;

public partial class RequestsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _currentUser;
    [ObservableProperty]
    private ObservableCollection<RequestModel> _requests;
}
