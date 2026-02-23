using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class EmployeeScheduleRepository : BaseRepository<EmployeeSchedule, int>, IEmployeeScheduleRepository
{
  public EmployeeScheduleRepository(CitasDbContext db) : base(db) { }

  public void Attach(Company company)
  {
    _db.Companies.Attach(company);
  }
}