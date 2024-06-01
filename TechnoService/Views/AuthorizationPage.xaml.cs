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
    private bool isRegisterPage = false;

    [GeneratedRegex(@"[^а-яА-Яa-zA-Z]")]
    private static partial Regex NameCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex LoginCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9#?!@$%^&*-]")]
    private static partial Regex PasswordCharsRegex();
    [GeneratedRegex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
    private static partial Regex PasswordRegex();

    private void TextCorrector(TextBox textBox, Regex regex)
    {
        var currentPosition = textBox.SelectionStart;
        textBox.Text = regex.Replace(textBox.Text, "");
        textBox.Select(currentPosition, 0);
        if (textBox.Text.Length > 0)
            textBox.BorderBrush = _textBoxDefaultBorderBrush;
        else
            textBox.BorderBrush = _textBoxUncorrectBorderBrush;
    }
    private void PasswordCorrector(PasswordBox passwordBox)
    {
        passwordBox.Password = PasswordCharsRegex().Replace(passwordBox.Password, "");
        if (!PasswordRegex().IsMatch(passwordBox.Password)) passwordBox.BorderBrush = _textBoxUncorrectBorderBrush;
        else passwordBox.BorderBrush = _textBoxDefaultBorderBrush;
        if (isRegisterPage)
        {
            PasswordBox repeatPasswordBox = (PasswordBox)AuthStackPanel.Children[6];
            if (!(Password.Password == repeatPasswordBox.Password))
                repeatPasswordBox.BorderBrush = _textBoxUncorrectBorderBrush;
            else repeatPasswordBox.BorderBrush = _textBoxDefaultBorderBrush;
        }
    }
    private TextBox GetNameBox(string header, string placeholder, bool required = true)
    {
        TextBox textBox = new() { Header = header, PlaceholderText = placeholder, MaxLength = 50 };
        if (required) textBox.BorderBrush = _textBoxUncorrectBorderBrush;
        textBox.TextChanging += OnNameChanging;
        return textBox;
    }
    private void ToLogin()
    {
        isRegisterPage = false;
        ChangeAuthButton.Click -= OnLoginPageButtonClick;
        ChangeAuthButton.Click += OnRegisterPageButtonClick;
        ChangeAuthButton.Content = "Зарегистрироваться";
        PageName.Text = "Вход";
        for (int i = 0; i < 3; i++) AuthStackPanel.Children.RemoveAt(1);
        AuthStackPanel.Children.RemoveAt(3);
        AuthButton.Content = "Войти";
        AuthButton.Click -= OnRegisterButtonClick;
        AuthButton.Click += OnLoginButtonClick;
    }

    private void OnNameChanging(TextBox sender, TextBoxTextChangingEventArgs args) =>
        TextCorrector(sender, NameCharsRegex());
    private void OnLoginChanging(TextBox sender, TextBoxTextChangingEventArgs args) =>
        TextCorrector(sender, LoginCharsRegex());
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args) =>
        PasswordCorrector(sender);
    private void OnRepeatPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        PasswordCorrector(sender);
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MainPage));
    }
    private async void OnRegisterButtonClick(object sender, RoutedEventArgs e)
    {
        string errorMessage = "";
        TextBox surnameBox = (TextBox)AuthStackPanel.Children[1];
        TextBox nameBox = (TextBox)AuthStackPanel.Children[2];
        TextBox patronymicBox = (TextBox)AuthStackPanel.Children[3];
        PasswordBox repeatPasswordBox = (PasswordBox)AuthStackPanel.Children[6];

        if (string.IsNullOrEmpty(surnameBox.Text))
            errorMessage += "Введите фамилию\n";
        if (string.IsNullOrEmpty(nameBox.Text))
            errorMessage += "Введите имя\n";
        if (string.IsNullOrEmpty(Login.Text))
            errorMessage += "Введите логин\n";
        if (!PasswordRegex().IsMatch(Password.Password))
            errorMessage += "Пароль не соответствует требованиям:\n" +
                "  -минимум 8 символов\n" +
                "  -cимволы верхнего и нижнего регистра\n" +
                "  -цифры\n" +
                "  -специальные символы(#?!@$%^&*-)\n";
        if (Password.Password != repeatPasswordBox.Password)
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

        _authorizationViewModel.RegisterCommand.Execute(new Models.UserModel()
        {
            Login = Login.Text,
            Password = Password.Password,
            Surname = surnameBox.Text,
            Name = nameBox.Text,
            Patronymic = patronymicBox.Text,
        });
        if (_authorizationViewModel.Error)
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "Ошибка",
                Content = "Ошибка регистрации",
                CloseButtonText = "Ок",
            }.ShowAsync();
            return;
        }

        ToLogin();
    }
    private void OnRegisterPageButtonClick(object sender, RoutedEventArgs e)
    {
        isRegisterPage = true;
        ChangeAuthButton.Click -= OnRegisterPageButtonClick;
        ChangeAuthButton.Click += OnLoginPageButtonClick;
        ChangeAuthButton.Content = "Войти";
        PageName.Text = "Регистрация";
        AuthStackPanel.Children.Insert(1, GetNameBox("Фамилия:", "Фамилия"));
        AuthStackPanel.Children.Insert(2, GetNameBox("Имя:", "Имя"));
        AuthStackPanel.Children.Insert(3, GetNameBox("Отчество:", "Отчество", false));
        PasswordBox passwordBox = new() { Header = "Повторите пароль:", PlaceholderText = "Пароль", MaxLength = 50, BorderBrush = _textBoxUncorrectBorderBrush };
        passwordBox.PasswordChanging += OnRepeatPasswordChanging;
        AuthStackPanel.Children.Insert(6, passwordBox);
        AuthButton.Content = "Зарегистрироваться";
        AuthButton.Click -= OnLoginButtonClick;
        AuthButton.Click += OnRegisterButtonClick;
    }
    private void OnLoginPageButtonClick(object sender, RoutedEventArgs e)
    {
        ToLogin();
    }
}
