using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class StatisticsPage : Page
{
    public StatisticsPage()
    {
        InitializeComponent();
        _viewModel.SelectedRequestType = _viewModel.RequestsTypes[0];
    }

    private readonly StatisticsPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Frame.BackStack.Clear();
        base.OnNavigatedTo(e);
    }
    private async void RequestsTypesSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await _viewModel.ComputeStatisticsCommand.ExecuteAsync(null);
    }
}
