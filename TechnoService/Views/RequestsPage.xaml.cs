using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace TechnoService.Views;

public sealed partial class RequestsPage : Page
{
    public RequestsPage()
    {
        InitializeComponent();
        for (int i = 0; i < 10; i++)
        {
            Requests.Add(new Request
            {
                Id = i,
                StartDate = DateTime.Now,
                ClientName = "Иванов Клиент Иванович",
                ExecutorName = "Иванов Исполнитель Иванович",
                Device = "Xiaomi Redmi Note 6",
                Type = "Телефон",
                FaultDescription = "Треснут экран",
                Status = "Выполнено"
            });
        }
    }

    public record Request
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string ClientName { get; set; }
        public string ExecutorName { get; set; }
        public string Device { get; set; }
        public string Type { get; set; }
        public string FaultDescription { get; set; }
        public string Status { get; set; }
    }

    public ObservableCollection<Request> Requests { get; set; } = [];
}
