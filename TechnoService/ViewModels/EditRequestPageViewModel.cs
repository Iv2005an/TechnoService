using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class EditRequestPageViewModel : ObservableObject
{
    [ObservableProperty]
    private RequestModel _request;
    [ObservableProperty]
    private ObservableCollection<UserModel> _executors = new(UsersService.GetExecutors());

    [RelayCommand]
    private async Task UpdateRequest() =>
        await RequestsService.UpdateRequest(Request);
}
