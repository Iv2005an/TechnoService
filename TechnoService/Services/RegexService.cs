using System.Text.RegularExpressions;

namespace TechnoService.Services;

public partial class RegexService
{
    [GeneratedRegex(@"[^а-яА-Яa-zA-Z]")]
    public static partial Regex NameCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    public static partial Regex LoginCharsRegex();
    [GeneratedRegex(@"[^a-zA-Z0-9#?!@$%^&*_-]")]
    public static partial Regex PasswordCharsRegex();
    [GeneratedRegex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*_-]).{8,}$")]
    public static partial Regex PasswordRegex();
}
