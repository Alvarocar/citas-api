using Citas.Application.Dto;
using Citas.Application.Schedules;
using Citas.Application.Schedules.Strategies;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;
using Citas.Tests.Helpers;
using Moq;

namespace Citas.Tests.Unit.Application.Services;

public class ScheduleServiceTests
{
  private readonly Mock<IEmployeeRepository> _mockEmployeeRepo = new();
  private readonly Mock<IScheduleCreateOneStrategyFactory> _mockStrategyFactory = new();
  private readonly Mock<IScheduleCreateOneStrategy> _mockStrategy = new();

  private readonly ScheduleService _sut;

  public ScheduleServiceTests()
  {
    _sut = new ScheduleService(
        _mockEmployeeRepo.Object,
        _mockStrategyFactory.Object);
  }

  // --- CreateOne ---

  [Fact]
  public async Task CreateOne_WhenEmployeeNotFound_ThrowsNotFoundException()
  {
    _mockEmployeeRepo
        .Setup(r => r.FindOne(It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Employee?)null);

    await Assert.ThrowsAsync<NotFoundException>(() =>
        _sut.CreateOne(TestEntities.AdminToken, TestDtos.MakeScheduleCreateDto(), default));
  }

  [Fact]
  public async Task CreateOne_WhenStrategyIsNull_ThrowsForbiddenException()
  {
    _mockEmployeeRepo
        .Setup(r => r.FindOne(It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(TestEntities.MakeEmployee());

    _mockStrategyFactory
        .Setup(f => f.GetStrategy(It.IsAny<UserTokenDto>()))
        .Returns((IScheduleCreateOneStrategy?)null);

    await Assert.ThrowsAsync<ForbiddenException>(() =>
        _sut.CreateOne(TestEntities.EmployeeToken, TestDtos.MakeScheduleCreateDto(), default));
  }

  [Fact]
  public async Task CreateOne_WithValidRequest_ReturnsSchedule()
  {
    var employee = TestEntities.MakeEmployee();
    var dto = TestDtos.MakeScheduleCreateDto("Morning Shift");
    var expected = new EmployeeSchedule { Id = 7, Name = "Morning Shift", Company = TestEntities.DefaultCompany };

    _mockEmployeeRepo
        .Setup(r => r.FindOne(It.IsAny<System.Linq.Expressions.Expression<Func<Employee, bool>>>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(employee);

    _mockStrategyFactory
        .Setup(f => f.GetStrategy(TestEntities.AdminToken))
        .Returns(_mockStrategy.Object);

    _mockStrategy
        .Setup(s => s.ExecuteAsync(dto, employee.Company, It.IsAny<CancellationToken>()))
        .ReturnsAsync(expected);

    var result = await _sut.CreateOne(TestEntities.AdminToken, dto, default);

    Assert.Equal(expected.Id, result.Id);
    Assert.Equal(expected.Name, result.Name);
    _mockStrategy.Verify(s => s.ExecuteAsync(dto, employee.Company, It.IsAny<CancellationToken>()), Times.Once);
  }
}
