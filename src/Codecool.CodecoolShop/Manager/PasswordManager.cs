using System;
using System.Security.Cryptography;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Manager;

public class PasswordManager
{
    public static HashSalt GenerateSaltedHash(string password)
    {
        var saltBytes = new byte[password.Length];
        var provider = RandomNumberGenerator.Create();
        provider.GetNonZeroBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
        var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

        var hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
        return hashSalt;
        
    }
    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
        return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
    }
}