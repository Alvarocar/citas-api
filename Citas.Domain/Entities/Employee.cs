namespace Citas.Domain.Entities
{
  public class Employee
  {
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public bool HasAccount { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public required Rol Rol { get; set; }

    public required Company Company { get; set; }

    public EmployeeSchedule? Schedule { get; set; }

    public Position? Position { get; set; }
  }
}
