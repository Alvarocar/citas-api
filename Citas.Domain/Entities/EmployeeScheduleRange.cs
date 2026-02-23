using Citas.Domain.Enums;

namespace Citas.Domain.Entities;
public class EmployeeScheduleRange
{
  public int Id { get; set; }

  public required EDay Day { get; set; }

  public required TimeOnly StartTime { get; set; }

  public required TimeOnly EndTime { get; set; }

  public required EmployeeSchedule Schedule { get; set; }
}

