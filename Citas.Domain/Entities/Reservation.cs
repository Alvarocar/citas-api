using Citas.Domain.Enums;
using Citas.Domain.ValueObj;

namespace Citas.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public required Client Client { get; set; }
        public required Employee Employee { get; set; }
        public required DateTimeRange RangeTime { get; set; }

       public double Price { get; set; }

       public string? ClientComment { get; set; }

       public float? RatingFromClient { get; set; }
    
       public string? EmployeeComment { get; set; }

       public float? RatingFromEmployee { get; set; }

       public required EReservationState State { get; set; }
    }
}
