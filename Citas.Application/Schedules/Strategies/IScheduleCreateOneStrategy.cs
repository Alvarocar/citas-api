using Citas.Application.Dto;
using Citas.Domain.Entities;

namespace Citas.Application.Schedules.Strategies;

public interface IScheduleCreateOneStrategy
{
  Task<EmployeeSchedule> ExecuteAsync(ScheduleCreateDto dto, Company company, CancellationToken ct);
}