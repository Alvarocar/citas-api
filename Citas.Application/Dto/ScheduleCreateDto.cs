using System.ComponentModel.DataAnnotations;
using Citas.Domain.Enums;

namespace Citas.Application.Dto;

public class ScheduleRangeDto
{
    [Required]
    public EDay Day { get; set; }

    [Required]
    public string StartTime { get; set; } = null!;

    [Required]
    public string EndTime { get; set; } = null!;
}

public class ScheduleCreateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public List<ScheduleRangeDto> Ranges { get; set; } = new();
}
