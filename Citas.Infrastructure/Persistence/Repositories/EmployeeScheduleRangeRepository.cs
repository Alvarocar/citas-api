using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;
public class EmployeeScheduleRangeRepository : BaseRepository<EmployeeScheduleRange, int>, IEmployeeScheduleRangeRepository
{
    public EmployeeScheduleRangeRepository(CitasDbContext db) : base(db)
    {
    }
}

