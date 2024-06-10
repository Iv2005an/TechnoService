using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TechnoService.Models;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class EditRequestPage : Page
{
    public EditRequestPage() => InitializeComponent();
    private readonly EditRequestPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        RequestModel editRequest = (RequestModel)e.Parameter;
        _viewModel.EditRequest = editRequest;
        base.OnNavigatedTo(e);
    }
}
