using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using TechnoService.Models;
using TechnoService.Services;

namespace TechnoService.ViewModels;

public partial class AddRequestPageViewModel : ObservableObject
{
    [ObservableProperty]
    private RequestModel _request = new() { Executor = new() { Id = 0 } };

    [RelayCommand]
    private async Task AddRequest() =>
        await RequestsService.AddRequest(Request);
}
