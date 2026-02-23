using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;

namespace Citas.Application.Employees.Strategies;

/// <summary>
/// 
/// </summary>
/// <exception cref="ForbiddenException">
/// The role of the employee cannot be created by the current user.
/// </exception>
/// <exception cref="NotFoundException">
///   The role or company was not found.
/// </exception>
public interface IEmployeeCreateOneStrategy
{
  Task<Employee> ExecuteAsync(EmployeeCreateDto dto, CancellationToken cancellationToken);
}

// TODO: this need to be moved to a service.
public class EmployeeCreateOneStrategy
  (
    IEmployeeRepository employeeRepository,
    IRolRepository rolRepository,
    ICompanyRepository companyRepository,
    IEmployeeFactory factory
  )
{
  public IEmployeeCreateOneStrategy? GetStrategy(UserTokenDto user)
  {
    return user.Role switch
    {
      Rol.SuperAdministrator => new ConcreteEmployeeCreateOneSuperAdmin(
                  user,
                  employeeRepository,
                  rolRepository,
                  companyRepository,
                  factory
                ),
      Rol.Administrator => new ConcreteEmployeeCreateOneAdmin(
                   user,
                  employeeRepository,
                  rolRepository,
                  companyRepository,
                  factory
                ),
      _ => null,
    };
  }
}

internal class ConcreteEmployeeCreateOneSuperAdmin(
    UserTokenDto user,
    IEmployeeRepository employeeRepository,
    IRolRepository rolRepository,
    ICompanyRepository companyRepository,
    IEmployeeFactory factory
  ) : IEmployeeCreateOneStrategy
{
  async public Task<Employee> ExecuteAsync(EmployeeCreateDto dto, CancellationToken ct)
  {
    if (dto.Role == Rol.SuperAdministrator) throw new ForbiddenException();

    var rol = await rolRepository.FirstOrDefaultAsync(rol => rol.Name == dto.Role, ct);
    var company = await companyRepository.FirstOrDefaultAsync(co => co.Id == user.CompanyId, ct);

    if (rol is null || company is null) throw new NotFoundException("Rol o Compañia no encontrada");

    if (dto.Email is not null)
    {
      var found = await employeeRepository.FindByEmail(dto.Email, ct);

      if (found is not null) throw new AlreadyExistException("Este correo ya esta registrado");
    }

    employeeRepository.AttachRol(rol);
    employeeRepository.AttachCompany(company);

    var employee = factory.Create(dto, rol, company);

    employeeRepository.Add(employee);

    await employeeRepository.SaveChangesAsync();

    return employee;
  }
}

internal class ConcreteEmployeeCreateOneAdmin(
    UserTokenDto user,
    IEmployeeRepository employeeRepository,
    IRolRepository rolRepository,
    ICompanyRepository companyRepository,
    IEmployeeFactory factory
  ) : IEmployeeCreateOneStrategy
{
  async public Task<Employee> ExecuteAsync(EmployeeCreateDto dto, CancellationToken ct)
  {
    if (dto.Role == Rol.SuperAdministrator || dto.Role == Rol.Administrator) throw new ForbiddenException();

    var rol = await rolRepository.FirstOrDefaultAsync(rol => rol.Name == dto.Role, ct);
    var company = await companyRepository.FirstOrDefaultAsync(co => co.Id == user.CompanyId, ct);

    if (rol is null || company is null) throw new NotFoundException("Rol o Compañia no encontrada");

    if (dto.Email is not null)
    {
      var found = await employeeRepository.FindByEmail(dto.Email, ct);

      if (found is not null) throw new AlreadyExistException("Este correo ya esta registrado");
    }

    employeeRepository.AttachRol(rol);
    employeeRepository.AttachCompany(company);

    var employee = factory.Create(dto, rol, company);

    employeeRepository.Add(employee);

    await employeeRepository.SaveChangesAsync();

    return employee;
  }
}

