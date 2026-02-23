namespace Citas.Application.Dto;

public class EmployeeScheduleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<ScheduleRangeDto> Ranges { get; set; } = new();
}