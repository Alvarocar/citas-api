using Citas.Domain.Entities;
using Citas.Domain.Filters;
using System.Linq.Expressions;

namespace Citas.Domain.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>
{
  void AttachRol(Rol rol);

  void AttachCompany(Company company);

  Task<Employee?> FindByEmail(string email, CancellationToken ct);

  Task<Employee?> FindById(int id, CancellationToken ct);

  Task<List<Employee>> FindAllByCompanyId(Employee employee, PaginationFilter pagination, CancellationToken ct);

  /// <summary>
  /// Should bring only one employee with company and rol included
  /// </summary>
  /// <param name="predicate"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Employee?> FindOne(Expression<Func<Employee, bool>> predicate, CancellationToken cancellationToken = default);
}