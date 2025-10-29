namespace Citas.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; }

        public required bool IsActive { get; set; }

        public required bool HasAccount { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public required Rol Rol { get; set; }
    
        public required Company Company { get; set; }

        public required EmployeeSchedule Schedule { get; set; }

        public Position? Position { get; set; }
    }
}
