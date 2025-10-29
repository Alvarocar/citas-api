using Citas.Domain.Enums;

namespace Citas.Domain.Entities
{
    public class EmployeeSchedule
    {
        public int Id { get; set; }

        public required EDay Day { get; set; }

        public required TimeOnly StartTime { get; set; }

        public required TimeOnly EndTime { get; set; }
    }
}
