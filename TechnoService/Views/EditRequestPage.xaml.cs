using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using TechnoService.Helpers;
using TechnoService.Models;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class EditRequestPage : Page
{
    public EditRequestPage() => InitializeComponent();

    private readonly EditRequestPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        RequestModel request = (RequestModel)e.Parameter;
        _viewModel.Request = request;
        ExecutorComboBox.SelectedIndex = _viewModel.Executors.IndexOf(
            _viewModel.Executors.Where(
                (executor) => executor.Id == request.Executor.Id).FirstOrDefault());
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
            errorMessage += "¬ведите название оборудовани€\n";
        if (string.IsNullOrEmpty(_viewModel.Request.Type))
            errorMessage += "¬ведите тип оборудовани€\n";
        if (string.IsNullOrEmpty(_viewModel.Request.Description))
            errorMessage += "¬ведите описание проблемы\n";
        errorMessage = errorMessage.Trim();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "ќшибка",
                Content = errorMessage,
                CloseButtonText = "ќк",
            }.ShowAsync();
        }
        else
        {
            await _viewModel.UpdateRequestCommand.ExecuteAsync(null);
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }

    private void ExecutorsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox comboBox = (ComboBox)sender;
        UserModel selectedExecutor = (UserModel)comboBox.SelectedItem;
        if (selectedExecutor.Type == UserTypes.Executor)
            _viewModel.Request.Status = StatusTypes.InProgress;
        else _viewModel.Request.Status = StatusTypes.Pending;
        Status.Text = _viewModel.Request.StatusName;
    }
}
