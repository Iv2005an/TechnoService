using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class UsersPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<UserModel> _users = new(UsersService.GetUsers());
}
