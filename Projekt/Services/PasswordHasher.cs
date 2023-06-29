using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);

            string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashedPassword;
        }
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        string passwordHash = HashPassword(password);
        return string.Equals(passwordHash, hashedPassword, StringComparison.OrdinalIgnoreCase);
    }
}