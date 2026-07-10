using System.Security.Cryptography;
using System.Text;

namespace APIForge.Shared.Security;

public static class ApiKeyHasher
{
    public static string GeneratePlainTextKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return "af_" + Convert.ToBase64String(bytes).Replace("+", "").Replace("/", "").Replace("=", "");
    }

    public static string Hash(string plainTextKey)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainTextKey));
        return Convert.ToHexString(bytes);
    }

    public static string Prefix(string plainTextKey)
    {
        return plainTextKey.Length <= 10 ? plainTextKey : plainTextKey[..10];
    }
}
