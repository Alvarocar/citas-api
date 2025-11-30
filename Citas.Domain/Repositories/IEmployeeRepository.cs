using Citas.Domain.Entities;
using Citas.Domain.Filters;

namespace Citas.Domain.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>
{
  void AttachRol(Rol rol);

  void AttachCompany(Company company);

  Task<Employee?> FindByEmail(string email, CancellationToken ct);

  Task<Employee?> FindById(int id, CancellationToken ct);

  Task<List<Employee>> FindAllByCompanyId(int company, PaginationFilter pagination, CancellationToken ct);
}