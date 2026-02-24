using Citas.Application.Dto;
using Citas.Application.Employees.Strategies;
using Citas.Application.Factories;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;
using Citas.Tests.Helpers;
using Moq;

namespace Citas.Tests.Unit.Application.Strategies;

public class EmployeeCreateOneStrategyTests
{
  private readonly Mock<IEmployeeRepository> _mockEmployeeRepo = new();
  private readonly Mock<IRolRepository> _mockRolRepo = new();
  private readonly Mock<ICompanyRepository> _mockCompanyRepo = new();
  private readonly Mock<IEmployeeFactory> _mockFactory = new();

  private readonly EmployeeCreateOneStrategy _sut;

  private static readonly Rol AdminRol = TestEntities.AdminRol;
  private static readonly Rol EmployeeRol = TestEntities.EmployeeRol;
  private static readonly Company Company = TestEntities.DefaultCompany;
  private static readonly UserTokenDto SuperAdminToken = TestEntities.SuperAdminToken;
  private static readonly UserTokenDto AdminToken = TestEntities.AdminToken;
  private static readonly UserTokenDto EmployeeToken = TestEntities.EmployeeToken;

  public EmployeeCreateOneStrategyTests()
  {
    _sut = new EmployeeCreateOneStrategy(
        _mockEmployeeRepo.Object,
        _mockRolRepo.Object,
        _mockCompanyRepo.Object,
        _mockFactory.Object);
  }

  // --- GetStrategy ---

  [Fact]
  public void GetStrategy_ForSuperAdmin_ReturnsNonNullStrategy()
  {
    var strategy = _sut.GetStrategy(SuperAdminToken);
    Assert.NotNull(strategy);
  }

  [Fact]
  public void GetStrategy_ForAdmin_ReturnsNonNullStrategy()
  {
    var strategy = _sut.GetStrategy(AdminToken);
    Assert.NotNull(strategy);
  }

  [Fact]
  public void GetStrategy_ForEmployee_ReturnsNull()
  {
    var strategy = _sut.GetStrategy(EmployeeToken);
    Assert.Null(strategy);
  }

  // --- SuperAdmin strategy ---

  [Fact]
  public async Task SuperAdmin_CreateEmployee_WithSuperAdminRole_ThrowsForbiddenException()
  {
    var dto = MakeDto(role: Rol.SuperAdministrator);
    var strategy = _sut.GetStrategy(SuperAdminToken)!;

    await Assert.ThrowsAsync<ForbiddenException>(() => strategy.ExecuteAsync(dto, default));
  }

  [Fact]
  public async Task SuperAdmin_CreateEmployee_WhenRolNotFound_ThrowsNotFoundException()
  {
    var dto = MakeDto(role: Rol.Employee);

    _mockRolRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Rol, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rol?)null);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Company);

    var strategy = _sut.GetStrategy(SuperAdminToken)!;

    await Assert.ThrowsAsync<NotFoundException>(() => strategy.ExecuteAsync(dto, default));
  }

  [Fact]
  public async Task SuperAdmin_CreateEmployee_WithDuplicateEmail_ThrowsAlreadyExistException()
  {
    var dto = MakeDto(role: Rol.Employee, email: "dup@test.com");
    var existingEmployee = new Employee { FirstName = "A", LastName = "B", Rol = EmployeeRol, Company = Company };

    _mockRolRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Rol, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(EmployeeRol);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Company);
    _mockEmployeeRepo.Setup(r => r.FindByEmail("dup@test.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existingEmployee);

    var strategy = _sut.GetStrategy(SuperAdminToken)!;

    await Assert.ThrowsAsync<AlreadyExistException>(() => strategy.ExecuteAsync(dto, default));
  }

  [Fact]
  public async Task SuperAdmin_CreateEmployee_HappyPath_ReturnsEmployee()
  {
    var dto = MakeDto(role: Rol.Employee, email: "new@test.com");
    var newEmployee = new Employee { FirstName = "New", LastName = "Emp", Rol = EmployeeRol, Company = Company };

    _mockRolRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Rol, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(EmployeeRol);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Company);
    _mockEmployeeRepo.Setup(r => r.FindByEmail("new@test.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Employee?)null);
    _mockFactory.Setup(f => f.Create(dto, EmployeeRol, Company)).Returns(newEmployee);

    var strategy = _sut.GetStrategy(SuperAdminToken)!;
    var result = await strategy.ExecuteAsync(dto, default);

    Assert.Equal(newEmployee, result);
    _mockEmployeeRepo.Verify(r => r.Add(newEmployee), Times.Once);
  }

  // --- Admin strategy ---

  [Fact]
  public async Task Admin_CreateEmployee_WithAdminRole_ThrowsForbiddenException()
  {
    var dto = MakeDto(role: Rol.Administrator);
    var strategy = _sut.GetStrategy(AdminToken)!;

    await Assert.ThrowsAsync<ForbiddenException>(() => strategy.ExecuteAsync(dto, default));
  }

  [Fact]
  public async Task Admin_CreateEmployee_WithSuperAdminRole_ThrowsForbiddenException()
  {
    var dto = MakeDto(role: Rol.SuperAdministrator);
    var strategy = _sut.GetStrategy(AdminToken)!;

    await Assert.ThrowsAsync<ForbiddenException>(() => strategy.ExecuteAsync(dto, default));
  }

  [Fact]
  public async Task Admin_CreateEmployee_WithDuplicateEmail_ThrowsAlreadyExistException()
  {
    var dto = MakeDto(role: Rol.Employee, email: "dup@test.com");
    var existingEmployee = new Employee { FirstName = "A", LastName = "B", Rol = EmployeeRol, Company = Company };

    _mockRolRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Rol, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(EmployeeRol);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Company);
    _mockEmployeeRepo.Setup(r => r.FindByEmail("dup@test.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existingEmployee);

    var strategy = _sut.GetStrategy(AdminToken)!;

    await Assert.ThrowsAsync<AlreadyExistException>(() => strategy.ExecuteAsync(dto, default));
  }

  [Fact]
  public async Task Admin_CreateEmployee_HappyPath_ReturnsEmployee()
  {
    var dto = MakeDto(role: Rol.Employee, email: "new@test.com");
    var newEmployee = new Employee { FirstName = "New", LastName = "Emp", Rol = EmployeeRol, Company = Company };

    _mockRolRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Rol, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(EmployeeRol);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Company);
    _mockEmployeeRepo.Setup(r => r.FindByEmail("new@test.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Employee?)null);
    _mockFactory.Setup(f => f.Create(dto, EmployeeRol, Company)).Returns(newEmployee);

    var strategy = _sut.GetStrategy(AdminToken)!;
    var result = await strategy.ExecuteAsync(dto, default);

    Assert.Equal(newEmployee, result);
    _mockEmployeeRepo.Verify(r => r.Add(newEmployee), Times.Once);
  }

  // --- Helpers ---

  private static EmployeeCreateDto MakeDto(string role, string? email = null) =>
      TestDtos.MakeEmployeeCreateDto(role, email);
}
