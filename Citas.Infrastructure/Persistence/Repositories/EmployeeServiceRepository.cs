using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class EmployeeServiceRepository : BaseRepository<EmployeeService, int>, IEmployeeServiceRepository
{
    public EmployeeServiceRepository(CitasDbContext db) : base(db) { }
}