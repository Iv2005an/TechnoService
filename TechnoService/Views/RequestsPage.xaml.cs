using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TechnoService.Models;
using TechnoService.Services;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class RequestsPage : Page
{
    public RequestsPage() => InitializeComponent();
    private readonly RequestsPageViewModel _viewModel = new();
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel currentUser = (UserModel)e.Parameter;
        _viewModel.CurrentUser = currentUser;
        _viewModel.Requests = new(await RequestsService.GetRequests());
        switch (currentUser.Type)
        {
            case UserTypes.Client:
                break;
            case UserTypes.Executor:
                break;
            case UserTypes.Admin:
                break;
        }
        base.OnNavigatedTo(e);
    }
}
