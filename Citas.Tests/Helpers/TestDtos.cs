using Citas.Application.Dto;

namespace Citas.Tests.Helpers;

/// <summary>
/// DTO factory methods shared across unit and integration tests.
/// Use partial class files (e.g. TestDtos.Reservations.cs) to add
/// DTO factories for new features without growing this file.
/// </summary>
public static partial class TestDtos
{
  /// <summary>
  /// Builds an EmployeeCreateAdminDto with a GUID-unique email and company.
  /// Suitable for integration tests where each call must produce a distinct record.
  /// </summary>
  public static EmployeeCreateAdminDto MakeCreateAdminDto(string emailSuffix = "") => new()
  {
    Firstname = "Test",
    Lastname = "Admin",
    Email = $"emp_admin_{Guid.NewGuid():N}{emailSuffix}@test.com",
    Password = TestConstants.AdminPassword,
    PhoneNumber = TestConstants.EmployeePhone,
    Company = new CompanyCreateDto
    {
      Name = $"Co {Guid.NewGuid():N}",
      Address = TestConstants.CompanyAddress,
      PhoneNumber = TestConstants.CompanyPhone,
      Email = $"co_{Guid.NewGuid():N}@test.com"
    }
  };

  /// <summary>
  /// Builds an EmployeeCreateAdminDto with fixed values.
  /// Suitable for unit tests that do not hit a real database.
  /// </summary>
  public static EmployeeCreateAdminDto MakeCreateAdminDtoFixed(string email = "admin@new.com") => new()
  {
    Firstname = "New",
    Lastname = "Admin",
    Email = email,
    Password = "P@ssword1",
    PhoneNumber = TestConstants.EmployeePhone,
    Company = new CompanyCreateDto
    {
      Name = "NewCo",
      Address = "Calle 2",
      PhoneNumber = "0987654321",
      Email = "newco@test.com"
    }
  };

  /// <summary>
  /// Builds an EmployeeCreateDto for strategy unit tests.
  /// </summary>
  public static EmployeeCreateDto MakeEmployeeCreateDto(string role, string? email = null) => new()
  {
    Firstname = "Test",
    Lastname = "Employee",
    PhoneNumber = TestConstants.EmployeePhone,
    Role = role,
    Email = email
  };

  /// <summary>
  /// Builds a ServiceCreateDto with sensible defaults.
  /// </summary>
  public static ServiceCreateDto MakeServiceCreateDto(string name = "Test Service") => new()
  {
    Name = name,
    Description = "A test service",
    SuggestedPrice = 25.0f,
    IsUnavailable = false
  };
}
