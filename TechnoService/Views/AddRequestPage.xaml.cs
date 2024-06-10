using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using TechnoService.Helpers;
using TechnoService.Models;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class AddRequestPage : Page
{
    public AddRequestPage(UserModel client)
    {
        InitializeComponent();
        _viewModel.Request.Client = client;
    }

    public readonly AddRequestPageViewModel _viewModel = new();

    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextHelper.TextCharsChecker(sender);
        if (sender.Text.Length > 0)
            sender.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        else
            sender.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
    }

    public async Task<RequestModel> AddRequest()
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(_viewModel.Request.Device))
            errorMessage += "������� �������� ������������\n";
        if (string.IsNullOrEmpty(_viewModel.Request.Type))
            errorMessage += "������� ��� ������������\n";
        if (string.IsNullOrEmpty(_viewModel.Request.Description))
            errorMessage += "������� �������� ��������\n";
        errorMessage = errorMessage.Trim();
        if (string.IsNullOrEmpty(errorMessage))
        {
            await _viewModel.AddRequestCommand.ExecuteAsync(null);
            return _viewModel.Request;
        }
        await new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Title = "������",
            Content = errorMessage,
            CloseButtonText = "��",
        }.ShowAsync();
        return null;
    }
}
