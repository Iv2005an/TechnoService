using Microsoft.UI.Xaml.Controls;

namespace TechnoService.Views;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        NavigationView.SelectedItem = RequestsMenuItem;
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
