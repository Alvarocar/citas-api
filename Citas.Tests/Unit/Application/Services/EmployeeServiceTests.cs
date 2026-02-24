using Citas.Application.Dto;
using Citas.Application.Employees.Strategies;
using Citas.Application.Factories;
using Citas.Application.Services;
using Citas.Domain.Entities;
using Citas.Domain.Enums;
using Citas.Domain.Exceptions;
using Citas.Domain.Filters;
using Citas.Domain.Repositories;
using Citas.Domain.ValueObj;
using Citas.Tests.Helpers;
using Moq;
using AppEmployeeService = Citas.Application.Services.EmployeeService;

namespace Citas.Tests.Unit.Application.Services;

public class EmployeeServiceTests
{
  private readonly Mock<IEmployeeRepository> _mockEmployeeRepo = new();
  private readonly Mock<IRolRepository> _mockRolRepo = new();
  private readonly Mock<ICompanyRepository> _mockCompanyRepo = new();
  private readonly Mock<IPasswordHasherService> _mockHasher = new();
  private readonly Mock<IEmployeeFactory> _mockFactory = new();
  private readonly Mock<IReservationRepository> _mockReservationRepo = new();
  private readonly Mock<IEmployeeScheduleExceptionRepository> _mockScheduleExceptionRepo = new();
  private readonly Mock<IEmployeeUpdateOneStrategyFactory> _mockUpdateStrategyFactory = new();
  private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

  private readonly AppEmployeeService _sut;

  public EmployeeServiceTests()
  {
    var createOneStrategy = new EmployeeCreateOneStrategy(
        _mockEmployeeRepo.Object,
        _mockRolRepo.Object,
        _mockCompanyRepo.Object,
        _mockFactory.Object);

    var getByIdStrategy = new EmployeeGetByIdStrategy(
        _mockEmployeeRepo.Object);

    _sut = new AppEmployeeService(
        _mockEmployeeRepo.Object,
        _mockRolRepo.Object,
        _mockCompanyRepo.Object,
        _mockHasher.Object,
        _mockFactory.Object,
        _mockReservationRepo.Object,
        _mockScheduleExceptionRepo.Object,
        createOneStrategy,
        getByIdStrategy,
        _mockUpdateStrategyFactory.Object,
        _mockUnitOfWork.Object);
  }

  // --- SignIn ---

  [Fact]
  public async Task SignIn_WithValidCredentials_ReturnsToken()
  {
    var employee = TestEntities.MakeEmployee();
    var expected = new UserTokenDto { Id = 1, Email = "emp@test.com", FirstName = "Test", LastName = "User", Role = Rol.SuperAdministrator, CompanyId = 1 };

    _mockEmployeeRepo.Setup(r => r.FindByEmail("emp@test.com", It.IsAny<CancellationToken>()))
                     .ReturnsAsync(employee);
    _mockHasher.Setup(h => h.VerifyHashedPassword("hashed", "correct_pass"))
               .Returns(true);
    _mockFactory.Setup(f => f.CreateToken(employee)).Returns(expected);

    var result = await _sut.SignIn(new EmployeeSignInDto { Email = "emp@test.com", Password = "correct_pass" }, default);

    Assert.Equal(expected.Email, result.Email);
  }

  [Fact]
  public async Task SignIn_WithUnknownEmail_ThrowsNotFoundException()
  {
    _mockEmployeeRepo.Setup(r => r.FindByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Employee?)null);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.SignIn(new EmployeeSignInDto { Email = "nobody@test.com", Password = "pass" }, default));
  }

  [Fact]
  public async Task SignIn_WithWrongPassword_ThrowsNotFoundException()
  {
    var employee = TestEntities.MakeEmployee();

    _mockEmployeeRepo.Setup(r => r.FindByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(employee);
    _mockHasher.Setup(h => h.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(false);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.SignIn(new EmployeeSignInDto { Email = "emp@test.com", Password = "wrong" }, default));
  }

  // --- CreateOneAdmin ---

  [Fact]
  public async Task CreateOneAdmin_WhenEmailAlreadyExistsInEmployees_ThrowsAlreadyExistException()
  {
    _mockEmployeeRepo.Setup(r => r.FindByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(TestEntities.MakeEmployee());

    await Assert.ThrowsAsync<AlreadyExistException>(() =>
        _sut.CreateOneAdmin(TestDtos.MakeCreateAdminDtoFixed(), default));
  }

  [Fact]
  public async Task CreateOneAdmin_WhenEmailAlreadyExistsInCompanies_ThrowsAlreadyExistException()
  {
    _mockEmployeeRepo.Setup(r => r.FindByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Employee?)null);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(TestEntities.DefaultCompany);

    await Assert.ThrowsAsync<AlreadyExistException>(() =>
        _sut.CreateOneAdmin(TestDtos.MakeCreateAdminDtoFixed(), default));
  }

  [Fact]
  public async Task CreateOneAdmin_WhenRolNotFound_ThrowsCitasInternalException()
  {
    _mockEmployeeRepo.Setup(r => r.FindByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Employee?)null);
    _mockCompanyRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Company, bool>>>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Company?)null);
    _mockRolRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Rol, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rol?)null);

    await Assert.ThrowsAsync<CitasInternalException>(() =>
        _sut.CreateOneAdmin(TestDtos.MakeCreateAdminDtoFixed(), default));
  }

  // --- GetAllEmployees ---

  [Fact]
  public async Task GetAllEmployees_WhenCurrentEmployeeNotFound_ThrowsNotFoundException()
  {
    var token = new UserTokenDto { Id = 99, CompanyId = 1, Role = Rol.Administrator, Email = "x@x.com", FirstName = "X", LastName = "Y" };

    _mockEmployeeRepo.Setup(r => r.FindById(99, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Employee?)null);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.GetAllEmployees(token, new PaginationFilter(), default));
  }

  [Fact]
  public async Task GetAllEmployees_WhenFound_ReturnsListOfOverviews()
  {
    var currentEmployee = TestEntities.MakeEmployee();
    var employees = new List<Employee> { TestEntities.MakeEmployee(1), TestEntities.MakeEmployee(2, "emp2@test.com") };

    _mockEmployeeRepo.Setup(r => r.FindById(TestEntities.AdminToken.Id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(currentEmployee);
    _mockEmployeeRepo.Setup(r => r.FindAllByCompanyId(currentEmployee, It.IsAny<PaginationFilter>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(employees);

    var result = await _sut.GetAllEmployees(TestEntities.AdminToken, new PaginationFilter(), default);

    Assert.Equal(2, result.Count);
  }

  // --- DeleteById ---

  [Fact]
  public async Task DeleteById_WhenReservationsExist_ThrowsReservationAssignedException()
  {
    var current = TestEntities.MakeEmployee();

    _mockEmployeeRepo.Setup(r => r.FirstOrDefaultWithIncludesAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<System.Linq.Expressions.Expression<Func<Employee, object>>[]>()))
        .ReturnsAsync(current);

    _mockReservationRepo.Setup(r => r.FindAsync(
            It.IsAny<System.Linq.Expressions.Expression<Func<Reservation, bool>>>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(new List<Reservation>
        {
                new Reservation
                {
                    Client = new Client { FirstName = "C", LastName = "L", PhoneNumber = "0" },
                    Employee = TestEntities.MakeEmployee(),
                    RangeTime = DateTimeRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddHours(1)),
                    State = EReservationState.PENDING
                }
        });

    await Assert.ThrowsAsync<ReservationAssignedException>(() =>
        _sut.DeleteById(2, TestEntities.AdminToken, default));
  }
}
