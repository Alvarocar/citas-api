namespace Citas.Domain.Entities
{
    public class EmployeeService
    {
        public int Id { get; set; }

        public float Rating { get; set; }

        public required Employee Employee { get; set; }

        public required Service Service { get; set; }
    }
}
