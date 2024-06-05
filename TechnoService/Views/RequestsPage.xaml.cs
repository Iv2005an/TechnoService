using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
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
        var result = await new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Title = "Добавление заявки",
            PrimaryButtonText = "Добавить заявку",
            CloseButtonText = "Отмена",
            DefaultButton = ContentDialogButton.Primary,
            Content = addRequestPage
        }.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            await addRequestPage.AddRequest();
            _viewModel.Requests = new(await RequestsService.GetRequests());
        }
    }
}
