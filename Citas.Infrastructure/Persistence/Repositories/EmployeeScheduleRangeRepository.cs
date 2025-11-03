using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;
public class EmployeeScheduleRangeRepository : BaseRepository<Company, int>, ICompanyRepository
{
    public EmployeeScheduleRangeRepository(CitasDbContext db) : base(db)
    {
    }
}

