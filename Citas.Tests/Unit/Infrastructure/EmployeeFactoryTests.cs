using Citas.Application.Dto;
using Citas.Application.Services;
using Citas.Domain.Entities;
using Citas.Infrastructure.Factories.Employees;
using Citas.Tests.Helpers;
using Moq;

namespace Citas.Tests.Unit.Infrastructure;

public class EmployeeFactoryTests
{
  private readonly Mock<IPasswordHasherService> _mockHasher = new();
  private readonly EmployeeFactory _sut;

  private static readonly Rol AdminRol = TestEntities.AdminRol;
  private static readonly Rol SuperAdminRol = TestEntities.SuperAdminRol;
  private static readonly Company Company = TestEntities.DefaultCompany;

  public EmployeeFactoryTests()
  {
    _mockHasher.Setup(h => h.HashPassword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
               .Returns("hashed_password");
    _sut = new EmployeeFactory(_mockHasher.Object);
  }

  [Fact]
  public void CreateAdmin_MapsAllFieldsFromDto()
  {
    var dto = new EmployeeCreateAdminDto
    {
      Firstname = "Juan",
      Lastname = "Perez",
      Email = "Juan.Perez@COMPANY.COM",
      Password = "secret",
      PhoneNumber = "3001234567",
      Company = new CompanyCreateDto { Name = "Co", Address = "Addr", PhoneNumber = "1", Email = "c@c.com" }
    };

    var employee = _sut.CreateAdmin(dto, SuperAdminRol, Company);

    Assert.Equal("Juan", employee.FirstName);
    Assert.Equal("Perez", employee.LastName);
    Assert.Equal("juan.perez@company.com", employee.Email);
    Assert.Equal("hashed_password", employee.Password);
    Assert.Equal("3001234567", employee.PhoneNumber);
    Assert.True(employee.IsActive);
    Assert.True(employee.HasAccount);
    Assert.Equal(SuperAdminRol, employee.Rol);
    Assert.Equal(Company, employee.Company);
  }

  [Fact]
  public void Create_WithEmail_SetsHasAccountTrue()
  {
    var dto = new EmployeeCreateDto
    {
      Firstname = "Ana",
      Lastname = "Lopez",
      Email = "Ana@COMPANY.COM",
      PhoneNumber = "3009876543",
      Role = Rol.Employee
    };

    var employee = _sut.Create(dto, AdminRol, Company);

    Assert.True(employee.HasAccount);
    Assert.Equal("ana@company.com", employee.Email);
  }

  [Fact]
  public void Create_WithNullEmail_SetsHasAccountFalse()
  {
    var dto = new EmployeeCreateDto
    {
      Firstname = "Carlos",
      Lastname = "Ruiz",
      Email = null,
      PhoneNumber = "3001112222",
      Role = Rol.Employee
    };

    var employee = _sut.Create(dto, AdminRol, Company);

    Assert.False(employee.HasAccount);
    Assert.Null(employee.Email);
  }

  [Fact]
  public void Create_MapsAllFieldsFromDto()
  {
    var dto = new EmployeeCreateDto
    {
      Firstname = "Maria",
      Lastname = "Garcia",
      PhoneNumber = "3005556666",
      Role = Rol.Employee
    };

    var employee = _sut.Create(dto, AdminRol, Company);

    Assert.Equal("Maria", employee.FirstName);
    Assert.Equal("Garcia", employee.LastName);
    Assert.Equal("3005556666", employee.PhoneNumber);
    Assert.True(employee.IsActive);
    Assert.Equal(AdminRol, employee.Rol);
    Assert.Equal(Company, employee.Company);
  }

  [Fact]
  public void CreateToken_FromEmployee_MapsAllFields()
  {
    var employee = new Employee
    {
      Id = 42,
      FirstName = "Pedro",
      LastName = "Sanchez",
      Email = "pedro@test.com",
      Rol = AdminRol,
      Company = Company
    };

    var token = _sut.CreateToken(employee);

    Assert.Equal(42, token.Id);
    Assert.Equal("Pedro", token.FirstName);
    Assert.Equal("Sanchez", token.LastName);
    Assert.Equal("pedro@test.com", token.Email);
    Assert.Equal(Rol.Administrator, token.Role);
    Assert.Equal(1, token.CompanyId);
  }
}
