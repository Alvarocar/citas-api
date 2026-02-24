using Citas.Infrastructure.Services;

namespace Citas.Tests.Unit.Infrastructure;

public class PasswordHasherServiceTests
{
  private readonly PasswordHasherService _sut = new();

  [Fact]
  public void HashAndVerify_WithCorrectPassword_ReturnsTrue()
  {
    var password = "MySecureP@ssword1";

    var hash = _sut.HashPassword(password);
    var result = _sut.VerifyHashedPassword(hash, password);

    Assert.True(result);
  }

  [Fact]
  public void Verify_WithWrongPassword_ReturnsFalse()
  {
    var hash = _sut.HashPassword("CorrectPassword");

    var result = _sut.VerifyHashedPassword(hash, "WrongPassword");

    Assert.False(result);
  }

  [Fact]
  public void Verify_WithTamperedHash_ReturnsFalse()
  {
    var hash = _sut.HashPassword("SomePassword");
    var parts = hash.Split('.');
    var tampered = $"{parts[0]}.AAAA{parts[1]}.{parts[2]}";

    var result = _sut.VerifyHashedPassword(tampered, "SomePassword");

    Assert.False(result);
  }

  [Fact]
  public void Hash_ProducesDifferentOutputEachCall()
  {
    var password = "SamePassword";

    var hash1 = _sut.HashPassword(password);
    var hash2 = _sut.HashPassword(password);

    Assert.NotEqual(hash1, hash2);
  }

  [Fact]
  public void Hash_OutputFollowsExpectedFormat()
  {
    var hash = _sut.HashPassword("AnyPassword");
    var parts = hash.Split('.');

    Assert.Equal(3, parts.Length);
    Assert.True(int.TryParse(parts[2], out _), "Third segment should be an integer (iterations)");
  }
}
