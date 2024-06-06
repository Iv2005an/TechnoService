using Microsoft.UI.Xaml.Controls;
using System.Text.RegularExpressions;

namespace TechnoService.Services;

public static class TextService
{
    public static void TextCharsChecker(TextBox textBox, Regex regex)
    {
        var currentPosition = textBox.SelectionStart;
        textBox.Text = regex.Replace(textBox.Text, "");
        textBox.Select(currentPosition, 0);
    }
    public static void PasswordCharsChecker(PasswordBox passwordBox) =>
        passwordBox.Password = RegexService.PasswordCharsRegex().Replace(passwordBox.Password, "");
}
