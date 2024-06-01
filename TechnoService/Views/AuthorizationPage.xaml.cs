using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Text.RegularExpressions;

namespace TechnoService.Views;

public sealed partial class AuthorizationPage : Page
{
    public AuthorizationPage() => InitializeComponent();

    [GeneratedRegex(@"[^а-яА-Яa-zA-Z]")]
    private static partial Regex NameCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z\d!""#$%&'()*+,.:;<=>?@^`{|}~_\-\\\/[\]]")]
    private static partial Regex LoginCharsRegex();

    private void OnNameChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        var currentPosition = sender.SelectionStart;
        sender.Text = NameCharsRegex().Replace(sender.Text, "");
        sender.Select(currentPosition, 0);
    }
    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        var currentPosition = sender.SelectionStart;
        sender.Text = LoginCharsRegex().Replace(sender.Text, "");
        sender.Select(currentPosition, 0);
    }
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        sender.Password = LoginCharsRegex().Replace(sender.Password, "");
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MainPage));
    }
    private void OnLoginPageButtonClick(object sender, RoutedEventArgs e)
    {
        ChangeAuthButton.Click -= OnLoginPageButtonClick;
        ChangeAuthButton.Click += OnRegisterPageButtonClick;
        ChangeAuthButton.Content = "Зарегистрироваться";
        PageName.Text = "Вход";
        for (int i = 0; i < 3; i++) AuthStackPanel.Children.RemoveAt(1);
        AuthStackPanel.Children.RemoveAt(3);
        AuthButton.Content = "Войти";
        AuthButton.Click -= OnLoginPageButtonClick;
        AuthButton.Click += OnLoginButtonClick;
    }
    private TextBox GetNameBox(string header, string placeholder)
    {
        TextBox textBox = new() { Header = header, PlaceholderText = placeholder, MaxLength = 50 };
        textBox.TextChanging += OnNameChanging;
        return textBox;
    }
    private void OnRegisterPageButtonClick(object sender, RoutedEventArgs e)
    {
        ChangeAuthButton.Click -= OnRegisterPageButtonClick;
        ChangeAuthButton.Click += OnLoginPageButtonClick;
        ChangeAuthButton.Content = "Войти";
        PageName.Text = "Регистрация";
        AuthStackPanel.Children.Insert(1, GetNameBox("Фамилия:", "Фамилия"));
        AuthStackPanel.Children.Insert(1, GetNameBox("Имя:", "Имя"));
        AuthStackPanel.Children.Insert(1, GetNameBox("Отчество:", "Отчество"));
        PasswordBox passwordBox = new() { Header = "Повторите пароль:", PlaceholderText = "Пароль", MaxLength = 50 };
        passwordBox.PasswordChanging += OnPasswordChanging;
        AuthStackPanel.Children.Insert(6, passwordBox);
        AuthButton.Content = "Зарегистрироваться";
        AuthButton.Click -= OnLoginButtonClick;
        AuthButton.Click += OnLoginPageButtonClick;
    }
}
