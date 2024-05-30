using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;

namespace TechnoService;

public sealed partial class MainWindow : WinUIEx.WindowEx
{
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon("Assets/TechnoService.ico");
        MainFrame.NavigationFailed += OnNavigationFailed;
        MainFrame.Navigate(typeof(Views.AuthorizationPage));
    }

    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new System.Exception($"Error navigating to page: {e.SourcePageType.FullName}");
    }
}
