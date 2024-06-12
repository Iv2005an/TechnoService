using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class RequestsPageViewModel : ObservableObject
{
    [ObservableProperty]
    private UserModel _currentUser;
    [ObservableProperty]
    private ObservableCollection<RequestModel> _requests;
    [ObservableProperty]
    private RequestModel _selectedRequest;
    [RelayCommand]
    private async Task UpdateRequest()
    {
        await RequestsService.UpdateRequest(SelectedRequest);
        await GetRequestsCommand.ExecuteAsync(null);
    }
    [RelayCommand]
    private async Task GetRequests()
    {
        if (CurrentUser.Type == UserTypes.Client)
            Requests = new(await RequestsService.GetRequests(
                $"client_id={CurrentUser.Id}"));
        else if (CurrentUser.Type == UserTypes.Executor)
            Requests = new(await RequestsService.GetRequests(
                $"client_id={CurrentUser.Id} OR executor_id={CurrentUser.Id}"));
        else
            Requests = new(await RequestsService.GetRequests());
    }
}
