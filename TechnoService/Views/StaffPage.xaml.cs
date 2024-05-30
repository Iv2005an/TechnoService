using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System;

namespace TechnoService.Views;

public sealed partial class StaffPage : Page
{
    public StaffPage()
    {
        InitializeComponent();
        Staff.Add(new Executor
        {
            Id = 0,
            Name = $"������ ������������� ��������",
            Type = "�������������",
        });
        for (int i = 1; i < 10; i++)
        {
            Staff.Add(new Executor
            {
                Id = i,
                Name = $"������ �����������{i} ��������",
                Type = "�����������",
            });
        }
    }

    public record Executor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public ObservableCollection<Executor> Staff { get; set; } = [];
}
