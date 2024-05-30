using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TechnoService.Views;

public sealed partial class AuthorizationPage : Page
{
    public AuthorizationPage() => InitializeComponent();

    [GeneratedRegex(@"[^a-zA-Z\d!""#$%&'()*+,.:;<=>?@^`{|}~_\-\\\/[\]]")]
    private static partial Regex LoginingCharsRegex();
    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        var currentPosition = sender.SelectionStart;
        sender.Text = LoginingCharsRegex().Replace(sender.Text, "");
        sender.Select(currentPosition, 0);
    }
    private void OnPasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
    {
        sender.Password = LoginingCharsRegex().Replace(sender.Password, "");
    }

    private void OnLoginButtonClick(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(MainPage));
    }
}
