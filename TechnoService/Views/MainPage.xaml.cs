using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TechnoService.Models;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class MainPage : Page
{
    public MainPage() => InitializeComponent();
    private readonly MainPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel currentUser = (UserModel)e.Parameter;
        _viewModel.CurrentUser = currentUser;
        switch (currentUser.Type)
        {
            case UserTypes.Client:
                StaffMenuItem.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                StatisticsMenuItem.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                break;
            case UserTypes.Executor:
                StatisticsMenuItem.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                break;
        }
        NavigationView.SelectedItem = RequestsMenuItem;
        base.OnNavigatedTo(e);
    }
    private void NavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        switch (((NavigationViewItem)sender.SelectedItem).Tag)
        {
            case "Requests":
                ContentFrame.Navigate(typeof(RequestsPage), _viewModel.CurrentUser);
                break;
            case "Staff":
                ContentFrame.Navigate(typeof(StaffPage));
                break;
            case "Statistics":
                ContentFrame.Navigate(typeof(StatisticsPage), _viewModel.CurrentUser);
                break;
            case "Settings":
                ContentFrame.Navigate(typeof(SettingsPage));
                break;
            default:
                ContentFrame.Navigate(typeof(RequestsPage), _viewModel.CurrentUser);
                break;
        }
    }
}
