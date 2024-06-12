using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using TechnoService.Helpers;
using TechnoService.Models;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class EditUserPage : Page
{
    public EditUserPage() => InitializeComponent();
    private readonly EditUserPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel user = (UserModel)e.Parameter;
        _viewModel.User = user;
        base.OnNavigatedTo(e);
    }
    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextHelper.NameCharsChecker(sender, RegexHelper.NameCharsRegex());
        if (sender.Name == "PatronymicBox" || sender.Text.Length > 0)
            sender.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        else
            sender.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
    }
    private void CancelButtonClick(object sender, RoutedEventArgs e)
    {
        if (Frame.CanGoBack) Frame.GoBack();
    }
    private async void EditButtonClick(object sender, RoutedEventArgs e)
    {
        string errorMessage = "";
        if (_viewModel.User.Id == 0 && _viewModel.User.Type != UserTypes.Admin)
            errorMessage = "Недопустима смена типа пользователя главному администратору";
        if (string.IsNullOrEmpty(_viewModel.User.Surname))
            errorMessage += "Введите фамилию\n";
        if (string.IsNullOrEmpty(_viewModel.User.Name))
            errorMessage += "Введите имя\n";
        errorMessage = errorMessage.Trim();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = errorMessage,
                CloseButtonText = "Ок",
            }.ShowAsync();
        }
        else
        {
            await _viewModel.UpdateUserCommand.ExecuteAsync(null);
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}
