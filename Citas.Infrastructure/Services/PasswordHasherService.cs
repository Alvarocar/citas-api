using Citas.Application.Services;
using System.Security.Cryptography;

namespace Citas.Infrastructure.Services;

public sealed class PasswordHasherService : IPasswordHasherService
{

  private const int _saltSize = 16;
  private const int _hashSize = 32;
  private const int _iterations = 100000;
  private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;

  public string HashPassword(string password, int saltSize = _saltSize, int iterations = _iterations)
  {
    byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
    byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, _hashAlgorithmName, _hashSize);
  
    return $"{Convert.ToHexString(salt)}.{Convert.ToHexString(hash)}.{iterations}";
  }

  public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
  {
    string[] parts = hashedPassword.Split('.');
    byte[] salt = Convert.FromHexString(parts[0]);
    byte[] hash = Convert.FromHexString(parts[1]);
    int iterations = int.Parse(parts[2]);

    byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, iterations, _hashAlgorithmName, hash.Length);

    return CryptographicOperations.FixedTimeEquals(hash, inputHash);
  }
}
