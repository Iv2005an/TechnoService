using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using TechnoService.Models;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        NavigationView.SelectedItem = RequestsMenuItem;
    }
    private readonly MainPageViewModel _mainPageViewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel currentUser = (UserModel)e.Parameter;
        _mainPageViewModel.CurrentUser = currentUser;
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
        base.OnNavigatedTo(e);
    }

    private void NavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        switch (((NavigationViewItem)sender.SelectedItem).Tag)
        {
            case "Requests":
                ContentFrame.Navigate(typeof(RequestsPage));
                break;
            case "Staff":
                ContentFrame.Navigate(typeof(StaffPage));
                break;
            case "Statistics":
                ContentFrame.Navigate(typeof(StatisticsPage));
                break;
            case "Settings":
                ContentFrame.Navigate(typeof(SettingsPage));
                break;
            default:
                ContentFrame.Navigate(typeof(RequestsPage));
                break;
        }
    }
}
