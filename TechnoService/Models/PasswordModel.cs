using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TechnoService.Models;

public partial class PasswordModel : ObservableObject
{
    [ObservableProperty]
    private string _passwordString;
    [ObservableProperty]
    private string _salt;
    [ObservableProperty]
    private string _hash;

    private static string GetSalt() => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    private static string GetSHA512Hash(string password, string passwordSalt)
    {
        const string constantSalt = "6DAC6031965A80BC68B80B8C7702038DC330A78E559DF0A598CD9BF34D8FE7A122337007F5E7839CC8AFE5AA25FE4506E29A87530E80E90D6FB8895FA9025753";

        byte[] strBytes = Encoding.UTF8.GetBytes(password ?? "");
        byte[] constantSaltBytes = Encoding.UTF8.GetBytes(constantSalt);
        byte[] saltBytes = Encoding.UTF8.GetBytes(passwordSalt);
        byte[] saltedStrBytes = new byte[strBytes.Length + constantSaltBytes.Length + saltBytes.Length];

        strBytes.CopyTo(saltedStrBytes, 0);
        constantSaltBytes.CopyTo(saltedStrBytes, strBytes.Length);
        saltBytes.CopyTo(saltedStrBytes, strBytes.Length + constantSaltBytes.Length);

        return Convert.ToHexString(SHA512.HashData(saltedStrBytes));
    }
    public void ComputeHash(string salt = null)
    {
        Salt = salt ?? GetSalt();
        Hash = GetSHA512Hash(PasswordString, Salt);
    }
}
