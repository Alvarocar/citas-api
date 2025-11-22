using Citas.Domain.Entities;

namespace Citas.Domain.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>
{
  void AttachRol(Rol rol);

  void AttachCompany(Company company);

  Task<Employee?> FindByEmail(string email, CancellationToken ct);
}