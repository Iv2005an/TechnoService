using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TechnoService.Models;
using TechnoService.Services;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class RequestsPage : Page
{
    public RequestsPage() => InitializeComponent();
    private readonly RequestsPageViewModel _viewModel = new();
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel currentUser = (UserModel)e.Parameter;
        _viewModel.CurrentUser = currentUser;
        _viewModel.Requests = new(await RequestsService.GetRequests());
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
    private async void AddRequestClick(object sender, RoutedEventArgs e)
    {
        AddRequestPage addRequestPage = new(_viewModel.CurrentUser);
        if (await new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Title = "Добавление заявки",
            PrimaryButtonText = "Добавить заявку",
            CloseButtonText = "Отмена",
            DefaultButton = ContentDialogButton.Primary,
            Content = addRequestPage
        }.ShowAsync() == ContentDialogResult.Primary)
        {
            await addRequestPage.AddRequest();
            _viewModel.Requests = new(await RequestsService.GetRequests());
        }
    }

    private void DataGridSorting(object sender, DataGridColumnEventArgs e)
    {
        Func<RequestModel, object> sorter = null;
        switch (e.Column.Tag)
        {
            case "Id":
                sorter = (request) => request.Id;
                break;
            case "StartDate":
                sorter = (request) => request.StartDate;
                break;
            case "Client":
                sorter = (request) => request.Client.FullName;
                break;
            case "Executor":
                sorter = (request) => request.Executor.FullName;
                break;
            case "Device":
                sorter = (request) => request.Device;
                break;
            case "Type":
                sorter = (request) => request.Type;
                break;
            case "Status":
                sorter = (request) => request.StatusName;
                break;
            default:
                break;
        }
        if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
        {
            _viewModel.Requests = new(_viewModel.Requests.OrderBy(sorter));
            e.Column.SortDirection = DataGridSortDirection.Ascending;
        }
        else
        {
            _viewModel.Requests = new(_viewModel.Requests.OrderByDescending(sorter));
            e.Column.SortDirection = DataGridSortDirection.Descending;
        }
        foreach (var dgColumn in RequestsDataGrid.Columns)
        {
            if (dgColumn.DisplayIndex != e.Column.DisplayIndex)
            {
                dgColumn.SortDirection = null;
            }
        }
    }

    private void RequestsSearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            if (!string.IsNullOrEmpty(sender.Text))
                RequestsDataGrid.ItemsSource = new ObservableCollection<RequestModel>(_viewModel.Requests.Where((request) => request.IsSuitable(sender.Text)));
            else RequestsDataGrid.ItemsSource = _viewModel.Requests;
    }
}
