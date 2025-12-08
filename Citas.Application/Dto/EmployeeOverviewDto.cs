namespace Citas.Application.Dto;

public class EmployeeOverviewDto
{
  public required int Id { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string? Email { get; set; }
  public string PhoneNumber { get; set; } = string.Empty;
  public string Role { get; set; } = string.Empty;
}
