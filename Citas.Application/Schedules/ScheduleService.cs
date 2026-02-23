using Citas.Application.Dto;
using Citas.Application.Schedules.Strategies;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;

namespace Citas.Application.Schedules;

public class ScheduleService(
    IEmployeeRepository employeeRepository,
    IScheduleCreateOneStrategyFactory createOneFactory
  )
{
  async public Task<EmployeeSchedule> CreateOne(UserTokenDto user, ScheduleCreateDto payload, CancellationToken ct)
  {
    var currentEmployee = await employeeRepository.FindOne(u => user.Id == u.Id, ct) ?? throw new NotFoundException("Empleado no encontrado");

    var strategy = createOneFactory.GetStrategy(user);

    if (strategy is null)
      throw new ForbiddenException();

    var schedule = await strategy.ExecuteAsync(payload, currentEmployee.Company, ct);

    return schedule;
  }
}
