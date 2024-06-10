using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using TechnoService.Helpers;
using TechnoService.Styles;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class AuthorizationPage : Page
{
    public AuthorizationPage() => InitializeComponent();

    private readonly AuthorizationPageViewModel _viewModel = new();
    private bool _isRegisterPage = false;

    private void ToLogin()
    {
        _isRegisterPage = false;
        ChangeAuthButton.Click -= OnLoginPageButtonClick;
        ChangeAuthButton.Click += OnRegisterPageButtonClick;
        ChangeAuthButton.Content = "Зарегистрироваться";
        PageName.Text = "Вход";
        SurnameBox.Visibility = Visibility.Collapsed;
        NameBox.Visibility = Visibility.Collapsed;
        PatronymicBox.Visibility = Visibility.Collapsed;
        RepeatPasswordBox.Visibility = Visibility.Collapsed;
        AuthButton.Content = "Войти";
        AuthButton.Click -= OnRegisterButtonClick;
        AuthButton.Click += OnLoginButtonClick;
    }

    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextHelper.NameCharsChecker(sender, RegexHelper.NameCharsRegex());
        if (sender.Name == "PatronymicBox" || sender.Text.Length > 0)
            sender.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        else
            sender.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
    }
    private async void OnLoginChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextHelper.NameCharsChecker(sender, RegexHelper.LoginCharsRegex());
        bool fieldUncorrect = false;
        if (sender.Text.Length == 0)
            fieldUncorrect = true;
        _viewModel.CurrentUser.Login = sender.Text;
        await _viewModel.IsLoginFreeCommand.ExecuteAsync(null);
        if (string.IsNullOrEmpty(_viewModel.CommandMessage))
        {
            if (!_isRegisterPage) fieldUncorrect = true;
        }
        else
        {
            if (_isRegisterPage) fieldUncorrect = true;
        }
        if (fieldUncorrect)
            sender.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
        else
            sender.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
    }
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        TextHelper.PasswordCharsChecker(sender);
        if (!RegexHelper.PasswordRegex().IsMatch(PasswordBox.Password))
            PasswordBox.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
        else PasswordBox.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        if (_isRegisterPage)
        {
            if (!(PasswordBox.Password == RepeatPasswordBox.Password))
                RepeatPasswordBox.BorderBrush = BorderBrushes.TextBoxUncorrectBorderBrush;
            else RepeatPasswordBox.BorderBrush = BorderBrushes.TextBoxDefaultBorderBrush;
        }
    }

    private async void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
#if DEBUG
        _viewModel.CurrentUser.Login = "admin";
        _viewModel.CurrentUser.Password.PasswordString = "Admin_1234";
#endif
        string errorMessage = "";
        if (string.IsNullOrEmpty(_viewModel.CurrentUser.Login))
            errorMessage += "Введите логин\n";
        else
        {
            await _viewModel.IsLoginFreeCommand.ExecuteAsync(null);
            if (string.IsNullOrEmpty(_viewModel.CommandMessage))
                errorMessage += "Логин не существует\n";
        }
        if (!RegexHelper.PasswordRegex().IsMatch(_viewModel.CurrentUser.Password.PasswordString))
            errorMessage += "Пароль не соответствует требованиям:\n" +
                "  -минимум 8 символов\n" +
                "  -cимволы верхнего и нижнего регистра\n" +
                "  -цифры\n" +
                "  -специальные символы(#?!@$%^&*-)\n";
        errorMessage = errorMessage.Trim();
        if (errorMessage.Length > 0)
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = errorMessage,
                CloseButtonText = "Ок",
            }.ShowAsync();
            return;
        }

        await _viewModel.LoginCommand.ExecuteAsync(null);
        if (!string.IsNullOrEmpty(_viewModel.CommandMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = _viewModel.CommandMessage,
                CloseButtonText = "Ок",
            }.ShowAsync();
            return;
        }

        Frame.Navigate(typeof(MainPage), _viewModel.CurrentUser);
    }
    private async void OnRegisterButtonClick(object sender, RoutedEventArgs e)
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(_viewModel.CurrentUser.Surname))
            errorMessage += "Введите фамилию\n";
        if (string.IsNullOrEmpty(_viewModel.CurrentUser.Name))
            errorMessage += "Введите имя\n";
        if (string.IsNullOrEmpty(_viewModel.CurrentUser.Login))
            errorMessage += "Введите логин\n";
        else
        {
            await _viewModel.IsLoginFreeCommand.ExecuteAsync(null);
            errorMessage += _viewModel.CommandMessage ?? "";
        }
        if (!RegexHelper.PasswordRegex().IsMatch(_viewModel.CurrentUser.Password.PasswordString))
            errorMessage += "Пароль не соответствует требованиям:\n" +
                "  -минимум 8 символов\n" +
                "  -cимволы верхнего и нижнего регистра\n" +
                "  -цифры\n" +
                "  -специальные символы(#?!@$%^&*-)\n";
        if (_viewModel.CurrentUser.Password.PasswordString != RepeatPasswordBox.Password)
            errorMessage += "Пароли не совпадают\n";
        errorMessage = errorMessage.Trim();
        if (errorMessage.Length > 0)
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = errorMessage,
                CloseButtonText = "Ок",
            }.ShowAsync();
            return;
        }

        await _viewModel.RegisterCommand.ExecuteAsync(null);
        if (!string.IsNullOrEmpty(_viewModel.CommandMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = _viewModel.CommandMessage,
                CloseButtonText = "Ок",
            }.ShowAsync();
            return;
        }
        RepeatPasswordBox.Password = "";

        ToLogin();
    }
    private void OnRegisterPageButtonClick(object sender, RoutedEventArgs e)
    {
        _isRegisterPage = true;
        ChangeAuthButton.Click -= OnRegisterPageButtonClick;
        ChangeAuthButton.Click += OnLoginPageButtonClick;
        ChangeAuthButton.Content = "Войти";
        PageName.Text = "Регистрация";
        SurnameBox.Visibility = Visibility.Visible;
        NameBox.Visibility = Visibility.Visible;
        PatronymicBox.Visibility = Visibility.Visible;
        RepeatPasswordBox.Visibility = Visibility.Visible;
        AuthButton.Content = "Зарегистрироваться";
        AuthButton.Click -= OnLoginButtonClick;
        AuthButton.Click += OnRegisterButtonClick;
    }
    private void OnLoginPageButtonClick(object sender, RoutedEventArgs e)
    {
        ToLogin();
    }
}
