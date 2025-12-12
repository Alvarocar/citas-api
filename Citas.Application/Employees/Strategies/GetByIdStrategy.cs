using Citas.Application.Dto;
using Citas.Domain.Entities;
using Citas.Domain.Repositories;

namespace Citas.Application.Employees.Strategies;

internal interface IEmployeeGetByIdStrategy
{
  Task<EmployeeOverviewDto?> ExecuteAsync(int id, CancellationToken cancellationToken);
}

// TODO: this need to be moved to a service.

/// <summary>
/// 
/// </summary>
/// <param name="user">This user needs to bring company object</param>
/// <param name="repository"></param>
internal class EmployeeGetByIdFactory(UserTokenDto user, IEmployeeRepository repository)
{
  public IEmployeeGetByIdStrategy? CreateStrategy()
  {
    if (user.Role == Rol.SuperAdministrator || user.Role == Rol.Administrator)
    {
      return new EmployeeGetByIdAdminStrategy(user, repository);
    }
    if (user.Role == Rol.Employee)
    {
      return new EmployeeGetByIdEmployeeStrategy(user, repository);
    }
    else
    {
      return null;
    }
  }
}

internal class EmployeeGetByIdAdminStrategy(
  UserTokenDto current,
  IEmployeeRepository _employeeRepository
  ) : IEmployeeGetByIdStrategy
{
  public async Task<EmployeeOverviewDto?> ExecuteAsync(int id, CancellationToken ct)
  {
    var employee = await _employeeRepository.FindOne(e => e.Id == id && e.Company.Id == current.CompanyId, ct);

    if (employee is null) return null;

    return new EmployeeOverviewDto
    {
      Id = employee.Id,
      FirstName = employee.FirstName,
      LastName = employee.LastName,
      Email = employee.Email,
      PhoneNumber = employee.PhoneNumber ?? string.Empty,
      Role = employee.Rol.Name,
    };
  }
}

internal class EmployeeGetByIdEmployeeStrategy(
  UserTokenDto current,
  IEmployeeRepository _employeeRepository
  ) : IEmployeeGetByIdStrategy
{
  public async Task<EmployeeOverviewDto?> ExecuteAsync(int id, CancellationToken ct)
  {
    if (id != current.Id) return null;

    var employeeSelf = await _employeeRepository.FindOne(
      e => e.Id == current.Id,
      ct
    );

    if (employeeSelf is null) return null;

    return new EmployeeOverviewDto
    {
      Id = employeeSelf.Id,
      FirstName = employeeSelf.FirstName,
      LastName = employeeSelf.LastName,
      Email = employeeSelf.Email,
      PhoneNumber = employeeSelf.PhoneNumber ?? string.Empty,
      Role = employeeSelf.Rol.Name
    };
  }
}