using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using TechnoService.Helpers;
using TechnoService.Models;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class EditRequestPage : Page
{
    public EditRequestPage()
    {
        InitializeComponent();
        ExecutorComboBox.SelectedIndex = 0;
    }

    private readonly EditRequestPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        RequestModel editRequest = (RequestModel)e.Parameter;
        _viewModel.Request = editRequest;
        ExecutorComboBox.SelectedItem = editRequest.Executor;
        base.OnNavigatedTo(e);
    }
    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextHelper.TextCharsChecker(sender);
        if (sender.Text.Length > 0)
            sender.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        else
            sender.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
    }

    private void CancelButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (Frame.CanGoBack) Frame.GoBack();
    }
    private async void EditButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(_viewModel.Request.Device))
            errorMessage += "������� �������� ������������\n";
        if (string.IsNullOrEmpty(_viewModel.Request.Type))
            errorMessage += "������� ��� ������������\n";
        if (string.IsNullOrEmpty(_viewModel.Request.Description))
            errorMessage += "������� �������� ��������\n";
        errorMessage = errorMessage.Trim();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "������",
                Content = errorMessage,
                CloseButtonText = "��",
            }.ShowAsync();
        }
        else
        {
            await _viewModel.UpdateRequestCommand.ExecuteAsync(null);
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}
