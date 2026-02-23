namespace Citas.Domain.Entities
{
    public class EmployeeSchedule
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required Company Company { get; set; }
    }
}
