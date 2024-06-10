using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace TechnoService.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage() => InitializeComponent();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Frame.BackStack.Clear();
        base.OnNavigatedTo(e);
    }
}
