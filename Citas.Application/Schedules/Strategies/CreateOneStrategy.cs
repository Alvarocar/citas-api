using Citas.Application.Dto;
using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Application.Schedules.Strategies;

public interface IScheduleCreateOneStrategyFactory
{
  IScheduleCreateOneStrategy? GetStrategy(UserTokenDto user);
}

public class ConcreteScheduleCreateOneAdmin : IScheduleCreateOneStrategy
{
  private readonly IEmployeeScheduleRepository _scheduleRepository;
  private readonly IEmployeeScheduleRangeRepository _rangeRepository;
  private readonly IUnitOfWork _unitOfWork;

  public ConcreteScheduleCreateOneAdmin(
      IEmployeeScheduleRepository scheduleRepository,
      IEmployeeScheduleRangeRepository rangeRepository,
      IUnitOfWork unitOfWork
  )
  {
    _scheduleRepository = scheduleRepository;
    _rangeRepository = rangeRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task<EmployeeSchedule> ExecuteAsync(ScheduleCreateDto dto, Company company, CancellationToken ct)
  {
    using (_unitOfWork)
    {
      await _unitOfWork.BeginTransactionAsync(ct);

      var schedule = new EmployeeSchedule
      {
        Name = dto.Name,
        Company = company
      };

      _scheduleRepository.Attach(company);
      _scheduleRepository.Add(schedule);

      var ranges = dto.Ranges.Select(r => new EmployeeScheduleRange
      {
        Day = r.Day,
        StartTime = TimeOnly.Parse(r.StartTime),
        EndTime = TimeOnly.Parse(r.EndTime),
        Schedule = schedule
      });

      foreach (var range in ranges)
      {
        _rangeRepository.Add(range);
      }

      await _unitOfWork.SaveChangesAsync(ct);
      await _unitOfWork.CommitTransactionAsync(ct);

      return schedule;
    }
  }
}