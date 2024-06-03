using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using TechnoService.Models;
using TechnoService.ViewModels;

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
                ClientName = "Иванов Иван Иванович",
                ExecutorName = $"Иванов Исполнитель{i} Иванович",
                Device = "Xiaomi Redmi Note 6",
                Type = "Телефон",
                FaultDescription = "Треснут экран",
                Status = "Выполнено"
            });
        }
    }
    private readonly RequestsPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel currentUser = (UserModel)e.Parameter;
        _viewModel.CurrentUser = currentUser;
        switch (currentUser.Type)
        {
            case UserTypes.Client:
                break;
            case UserTypes.Executor:
                break;
            case UserTypes.Admin:
                break;
        }
        base.OnNavigatedTo(e);
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
