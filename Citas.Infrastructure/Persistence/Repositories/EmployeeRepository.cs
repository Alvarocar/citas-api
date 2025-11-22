using Citas.Domain.Entities;
using Citas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

  public Task<Employee?> FindByEmail(string email, CancellationToken ct)
  {
    return _set.Where(e => e.Email != null)
      .Where(e => e.Email! == email.ToLower())
      .Include(e => e.Rol)
      .FirstOrDefaultAsync(ct);
  }
}