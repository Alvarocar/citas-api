using Citas.Domain.Entities;
using Citas.Domain.Filters;
using Citas.Domain.Repositories;
using Citas.Infrastructure.Specifications;
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

  public Task<Employee?> FindById(int id, CancellationToken ct)
  {
    return _set.Where(e => e.Id == id)
      .Include(e => e.Rol)
      .Include(e => e.Company)
      .FirstOrDefaultAsync(ct);
  }

  public Task<List<Employee>> FindAllByCompanyId(Employee employee, PaginationFilter pagination, CancellationToken ct)
  {
    var query = _set.Include(e => e.Rol).Where(e => e.Company.Id == employee.Company.Id && e.Id != employee.Id);
    return new PaginationSpecification<Employee>(pagination).Apply(query).ToListAsync(ct);
  }
}