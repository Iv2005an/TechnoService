using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using TechnoService.Helpers;
using TechnoService.Models;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage() => this.InitializeComponent();
    private readonly SettingsPageViewModel _viewModel = new();
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        UserModel currentUser = (UserModel)e.Parameter;
        _viewModel.CurrentUser = currentUser;
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
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        TextHelper.PasswordCharsChecker(sender);
        if (!string.IsNullOrEmpty(PasswordBox.Password) &&
            !RegexHelper.PasswordRegex().IsMatch(PasswordBox.Password))
            PasswordBox.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
        else PasswordBox.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        if (!(PasswordBox.Password == RepeatPasswordBox.Password))
            RepeatPasswordBox.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
        else RepeatPasswordBox.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
    }
    private async void EditButtonClick(object sender, RoutedEventArgs e)
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(_viewModel.CurrentUser.Surname))
            errorMessage += "Введите фамилию\n";
        if (string.IsNullOrEmpty(_viewModel.CurrentUser.Name))
            errorMessage += "Введите имя\n";
        if (!string.IsNullOrEmpty(_viewModel.NewPassword))
        {
            bool passwordError = false;
            if (_viewModel.NewPassword != RepeatPasswordBox.Password)
            {
                errorMessage += "Пароли не совпадают\n";
                passwordError = true;
            }
            if (!RegexHelper.PasswordRegex().IsMatch(_viewModel.NewPassword))
            {
                errorMessage += "Пароль не соответствует требованиям:\n" +
                    "  -минимум 8 символов\n" +
                    "  -cимволы верхнего и нижнего регистра\n" +
                    "  -цифры\n" +
                    "  -специальные символы(#?!@$%^&*-)\n";
                passwordError = true;
            }
            if (!passwordError)
            {
                {
                    _viewModel.CurrentUser.Password.PasswordString = _viewModel.NewPassword;
                    _viewModel.CurrentUser.Password.ComputeHash();
                }
            }
        }
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
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Успешно",
                Content = "Данные изменены",
                CloseButtonText = "Ок",
            }.ShowAsync();
        }
    }
}
