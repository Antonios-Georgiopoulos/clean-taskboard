using System.Security.Cryptography;
using CleanTaskBoard.Application.Auth;

namespace CleanTaskBoard.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 100_000;

    public (string Hash, string Salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[SaltSize];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256
        );
        var hash = Convert.ToBase64String(pbkdf2.GetBytes(KeySize));

        return (hash, salt);
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256
        );
        var computedHash = Convert.ToBase64String(pbkdf2.GetBytes(KeySize));

        return computedHash == hash;
    }
}
