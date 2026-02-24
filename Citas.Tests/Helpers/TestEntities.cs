using Citas.Application.Dto;
using Citas.Domain.Entities;

namespace Citas.Tests.Helpers;

/// <summary>
/// Pre-built domain entity instances and factories shared across unit tests.
/// Use partial class files (e.g. TestEntities.Reservations.cs) to add
/// entities for new aggregates without growing this file.
/// </summary>
public static partial class TestEntities
{
  public static readonly Rol AdminRol = new() { Id = Rol.AdministratorId, Name = Rol.Administrator };
  public static readonly Rol EmployeeRol = new() { Id = Rol.EmployeeId, Name = Rol.Employee };
  public static readonly Rol SuperAdminRol = new() { Id = Rol.SuperAdministratorId, Name = Rol.SuperAdministrator };

  public static readonly Company DefaultCompany = new()
  {
    Id = 1,
    Name = "Acme",
    Address = TestConstants.CompanyAddress,
    Email = "acme@test.com",
    PhoneNumber = TestConstants.UnitCompanyPhone
  };

  public static Company MakeCompany(int id, string name = "Acme", string email = "acme@test.com") => new()
  {
    Id = id,
    Name = name,
    Address = TestConstants.CompanyAddress,
    Email = email,
    PhoneNumber = TestConstants.UnitCompanyPhone
  };

  public static Employee MakeEmployee(int id = 1, string email = "emp@test.com") => new()
  {
    Id = id,
    FirstName = "Test",
    LastName = "User",
    Email = email,
    Password = "hashed",
    IsActive = true,
    HasAccount = true,
    Rol = SuperAdminRol,
    Company = DefaultCompany
  };

  public static readonly UserTokenDto SuperAdminToken = new()
  {
    Id = 1,
    CompanyId = 1,
    Role = Rol.SuperAdministrator,
    Email = "super@test.com",
    FirstName = "Super",
    LastName = "Admin"
  };

  public static readonly UserTokenDto AdminToken = new()
  {
    Id = 1,
    CompanyId = 1,
    Role = Rol.Administrator,
    Email = "admin@test.com",
    FirstName = "Admin",
    LastName = "User"
  };

  public static readonly UserTokenDto EmployeeToken = new()
  {
    Id = 3,
    CompanyId = 1,
    Role = Rol.Employee,
    Email = "emp@test.com",
    FirstName = "Emp",
    LastName = "User"
  };
}
