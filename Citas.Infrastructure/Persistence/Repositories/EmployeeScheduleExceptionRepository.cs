using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class EmployeeScheduleExceptionRepository : BaseRepository<EmployeeScheduleException, int>, IEmployeeScheduleExceptionRepository
{
    public EmployeeScheduleExceptionRepository(CitasDbContext db) : base(db) { }
}