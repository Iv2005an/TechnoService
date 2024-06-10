using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace TechnoService.Views;

public sealed partial class StaffPage : Page
{
    public StaffPage()
    {
        InitializeComponent();
        Staff.Add(new Executor
        {
            Id = 0,
            Name = $"Иванов Администратор Иванович",
            Type = "Администратор",
        });
        for (int i = 1; i < 10; i++)
        {
            Staff.Add(new Executor
            {
                Id = i,
                Name = $"Иванов Исполнитель{i} Иванович",
                Type = "Исполнитель",
            });
        }
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        Frame.BackStack.Clear();
        base.OnNavigatedTo(e);
    }

    public record Executor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public ObservableCollection<Executor> Staff { get; set; } = [];
}
