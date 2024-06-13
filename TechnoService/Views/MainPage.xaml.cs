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
            case UserTypes.Client or UserTypes.Executor:
                StaffMenuItem.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                StatisticsMenuItem.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                break;
        }
        NavigationView.SelectedItem = RequestsMenuItem;
        base.OnNavigatedTo(e);
    }
    private void NavigationBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (ContentFrame.CanGoBack) ContentFrame.GoBack();
    }
    private void NavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        switch (((NavigationViewItem)sender.SelectedItem).Tag)
        {
            case "Requests":
                ContentFrame.Navigate(typeof(RequestsPage), _viewModel.CurrentUser);
                break;
            case "Users":
                ContentFrame.Navigate(typeof(UsersPage));
                break;
            case "Statistics":
                ContentFrame.Navigate(typeof(StatisticsPage));
                break;
            case "Settings":
                ContentFrame.Navigate(typeof(SettingsPage), _viewModel.CurrentUser);
                break;
            default:
                ContentFrame.Navigate(typeof(RequestsPage), _viewModel.CurrentUser);
                break;
        }
    }
}
