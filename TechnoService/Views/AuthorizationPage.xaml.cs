using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Text.RegularExpressions;
using TechnoService.ViewModels;

namespace TechnoService.Views;

public sealed partial class AuthorizationPage : Page
{
    public AuthorizationPage()
    {
        InitializeComponent();
        _authorizationViewModel = new AuthorizationViewModel();
        _textBoxDefaultBorderBrush = (LinearGradientBrush)new TextBox().BorderBrush;
    }

    private readonly AuthorizationViewModel _authorizationViewModel;
    private readonly LinearGradientBrush _textBoxDefaultBorderBrush;
    private static readonly SolidColorBrush _textBoxUncorrectBorderBrush = new() { Color = Colors.Red };
    private bool _isRegisterPage = false;

    [GeneratedRegex(@"[^а-яА-Яa-zA-Z]")]
    private static partial Regex NameCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex LoginCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9#?!@$%^&*-]")]
    private static partial Regex PasswordCharsRegex();
    [GeneratedRegex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    private static partial Regex PasswordRegex();

    private static void TextCorrector(TextBox textBox, Regex regex)
    {
        var currentPosition = textBox.SelectionStart;
        textBox.Text = regex.Replace(textBox.Text, "");
        textBox.Select(currentPosition, 0);
    }
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

    private void OnNameChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextCorrector(sender, NameCharsRegex());
        if (sender.Name == "PatronymicBox" || sender.Text.Length > 0)
            sender.BorderBrush = _textBoxDefaultBorderBrush;
        else
            sender.BorderBrush = _textBoxUncorrectBorderBrush;
    }
    private async void OnLoginChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        TextCorrector(sender, LoginCharsRegex());
        bool fieldUncorrect = false;
        if (sender.Text.Length == 0)
            fieldUncorrect = true;
        _authorizationViewModel.CurrentUser.Login = sender.Text;
        await _authorizationViewModel.IsLoginFreeCommand.ExecuteAsync(null);
        if (!string.IsNullOrEmpty(_authorizationViewModel.CommandMessage)) fieldUncorrect = true;
        if (fieldUncorrect)
            sender.BorderBrush = _textBoxUncorrectBorderBrush;
        else
            sender.BorderBrush = _textBoxDefaultBorderBrush;
    }
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        PasswordBox.Password = PasswordCharsRegex().Replace(PasswordBox.Password, "");
        if (!PasswordRegex().IsMatch(PasswordBox.Password)) PasswordBox.BorderBrush = _textBoxUncorrectBorderBrush;
        else PasswordBox.BorderBrush = _textBoxDefaultBorderBrush;
        if (_isRegisterPage)
        {
            if (!(PasswordBox.Password == RepeatPasswordBox.Password))
                RepeatPasswordBox.BorderBrush = _textBoxUncorrectBorderBrush;
            else RepeatPasswordBox.BorderBrush = _textBoxDefaultBorderBrush;
        }
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
    }
    private async void OnRegisterButtonClick(object sender, RoutedEventArgs e)
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(SurnameBox.Text))
            errorMessage += "Введите фамилию\n";
        if (string.IsNullOrEmpty(NameBox.Text))
            errorMessage += "Введите имя\n";
        if (string.IsNullOrEmpty(LoginBox.Text))
            errorMessage += "Введите логин\n";
        else
        {
            await _authorizationViewModel.IsLoginFreeCommand.ExecuteAsync(LoginBox.Text);
            errorMessage += _authorizationViewModel.CommandMessage ?? "";
        }
        if (!PasswordRegex().IsMatch(PasswordBox.Password))
            errorMessage += "Пароль не соответствует требованиям:\n" +
                "  -минимум 8 символов\n" +
                "  -cимволы верхнего и нижнего регистра\n" +
                "  -цифры\n" +
                "  -специальные символы(#?!@$%^&*-)\n";
        if (PasswordBox.Password != RepeatPasswordBox.Password)
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

        _authorizationViewModel.RegisterCommand.Execute(null);
        if (!string.IsNullOrEmpty(_authorizationViewModel.CommandMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = _authorizationViewModel.CommandMessage,
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
