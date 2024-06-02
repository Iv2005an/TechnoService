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

    [GeneratedRegex(@"[^�-��-�a-zA-Z]")]
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
        if (textBox.Name == "PatronymicBox" || textBox.Text.Length > 0)
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
            if (!(PasswordBox.Password == repeatPasswordBox.Password))
                repeatPasswordBox.BorderBrush = _textBoxUncorrectBorderBrush;
            else repeatPasswordBox.BorderBrush = _textBoxDefaultBorderBrush;
        }
    }
    private void ToLogin()
    {
        isRegisterPage = false;
        ChangeAuthButton.Click -= OnLoginPageButtonClick;
        ChangeAuthButton.Click += OnRegisterPageButtonClick;
        ChangeAuthButton.Content = "������������������";
        PageName.Text = "����";
        SurnameBox.Visibility = Visibility.Collapsed;
        NameBox.Visibility = Visibility.Collapsed;
        PatronymicBox.Visibility = Visibility.Collapsed;
        RepeatPasswordBox.Visibility = Visibility.Collapsed;
        AuthButton.Content = "�����";
        AuthButton.Click -= OnRegisterButtonClick;
        AuthButton.Click += OnLoginButtonClick;
    }

    private void OnNameChanging(TextBox sender, TextBoxTextChangingEventArgs args) =>
        TextCorrector(sender, NameCharsRegex());
    private void OnLoginChanging(TextBox sender, TextBoxTextChangingEventArgs args) =>
        TextCorrector(sender, LoginCharsRegex());
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args) =>
        PasswordCorrector(sender);

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
    }
    private async void OnRegisterButtonClick(object sender, RoutedEventArgs e)
    {
        string errorMessage = "";
        if (string.IsNullOrEmpty(SurnameBox.Text))
            errorMessage += "������� �������\n";
        if (string.IsNullOrEmpty(NameBox.Text))
            errorMessage += "������� ���\n";
        if (string.IsNullOrEmpty(LoginBox.Text))
            errorMessage += "������� �����\n";
        else
        {
            await _authorizationViewModel.IsLoginFreeCommand.ExecuteAsync(LoginBox.Text);
            errorMessage += _authorizationViewModel.CommandMessage ?? "";
        }
        if (!PasswordRegex().IsMatch(PasswordBox.Password))
            errorMessage += "������ �� ������������� �����������:\n" +
                "  -������� 8 ��������\n" +
                "  -c������ �������� � ������� ��������\n" +
                "  -�����\n" +
                "  -����������� �������(#?!@$%^&*-)\n";
        if (PasswordBox.Password != RepeatPasswordBox.Password)
            errorMessage += "������ �� ���������\n";
        errorMessage = errorMessage.Trim();
        if (errorMessage.Length > 0)
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "������",
                Content = errorMessage,
                CloseButtonText = "��",
            }.ShowAsync();
            return;
        }

        _authorizationViewModel.RegisterCommand.Execute(null);
        if (!string.IsNullOrEmpty(_authorizationViewModel.CommandMessage))
        {
            await new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Title = "������",
                Content = _authorizationViewModel.CommandMessage,
                CloseButtonText = "��",
            }.ShowAsync();
            return;
        }
        RepeatPasswordBox.Password = "";

        ToLogin();
    }
    private void OnRegisterPageButtonClick(object sender, RoutedEventArgs e)
    {
        isRegisterPage = true;
        ChangeAuthButton.Click -= OnRegisterPageButtonClick;
        ChangeAuthButton.Click += OnLoginPageButtonClick;
        ChangeAuthButton.Content = "�����";
        PageName.Text = "�����������";
        SurnameBox.Visibility = Visibility.Visible;
        NameBox.Visibility = Visibility.Visible;
        PatronymicBox.Visibility = Visibility.Visible;
        RepeatPasswordBox.Visibility = Visibility.Visible;
        AuthButton.Content = "������������������";
        AuthButton.Click -= OnLoginButtonClick;
        AuthButton.Click += OnRegisterButtonClick;
    }
    private void OnLoginPageButtonClick(object sender, RoutedEventArgs e)
    {
        ToLogin();
    }
}
