using Citas.Domain.ValueObj;

namespace Citas.Domain.Entities
{
    public class EmployeeScheduleException
    {
        public int Id { get; set; }
        public required Employee Employee { get; set; }
        public required DateTimeRange RangeTime { get; set; }
        public required string Comment { get; set; }
    }
}
