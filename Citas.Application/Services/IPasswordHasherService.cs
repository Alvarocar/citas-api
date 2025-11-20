namespace Citas.Application.Services;

public interface IPasswordHasherService
{
  string HashPassword(string password, int saltSize = 16, int iterations = 10000);
  bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}