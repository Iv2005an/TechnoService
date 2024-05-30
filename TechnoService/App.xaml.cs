using Microsoft.UI.Xaml;

namespace TechnoService;

public partial class App : Application
{
    public App() => InitializeComponent();

    private WinUIEx.WindowEx m_window;

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }
}
