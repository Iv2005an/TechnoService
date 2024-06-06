using Microsoft.UI.Xaml.Controls;
using System.Text.RegularExpressions;

namespace TechnoService.Services;

public static class TextService
{
    public static void TextCharsChecker(TextBox textBox)
    {
        var currentPosition = textBox.SelectionStart;
        textBox.Text = textBox.Text.TrimStart().Replace("  ", " ");
        textBox.Select(currentPosition, 0);
    }

    public static void NameCharsChecker(TextBox textBox, Regex regex)
    {
        var currentPosition = textBox.SelectionStart;
        textBox.Text = regex.Replace(textBox.Text, "");
        textBox.Select(currentPosition, 0);
    }
    public static void PasswordCharsChecker(PasswordBox passwordBox) =>
        passwordBox.Password = RegexService.PasswordCharsRegex().Replace(passwordBox.Password, "");
}
