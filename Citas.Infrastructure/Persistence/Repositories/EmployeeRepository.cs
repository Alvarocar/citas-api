using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee, int>, IEmployeeRepository
{
  public EmployeeRepository(CitasDbContext db) : base(db) { }

  public void AttachRol(Rol rol)
  {
    _db.Rols.Attach(rol);
  }

  public void AttachCompany(Company company)
  {
    _db.Companies.Attach(company);
  }
}