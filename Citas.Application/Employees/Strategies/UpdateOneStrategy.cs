using Citas.Application.Dto;
using Citas.Domain.Entities;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;

namespace Citas.Application.Employees.Strategies;

public interface IEmployeeUpdateOneStrategy
{
  /// <summary>
  /// Excecute the strategy to update an employee. The strategy will be selected based on the user's role and the employee's id.
  /// </summary>
  /// <param name="user">The current user that makes the request</param>
  /// <param name="id">The user id to update</param>
  /// <param name="dto">The payload to update the user</param>
  /// <param name="ct"></param>
  /// <returns></returns>
  Task<Employee> ExecuteAsync(UserTokenDto user, int id, EmployeeUpdateDto dto, CancellationToken ct);
}

public interface IEmployeeUpdateOneStrategyFactory
{
  IEmployeeUpdateOneStrategy? GetStrategy(UserTokenDto user);
}

public class ConcreteEmployeeUpdateEmployee(
    IEmployeeRepository employeeRepository,
    IRolRepository rolRepository
  ) : IEmployeeUpdateOneStrategy
{
  async public Task<Employee> ExecuteAsync(UserTokenDto user, int id, EmployeeUpdateDto dto, CancellationToken ct)
  {
    if (user.Id != id) throw new ForbiddenException("Solo puede editar su propio usuario");

    var employee = await employeeRepository.FindOne(e => e.Id == id, ct)
      ?? throw new NotFoundException("Empleado no encontrado");

    ApplyUpdate(employee, dto, ct);

    await employeeRepository.SaveChangesAsync(ct);

    return employee;
  }

  private void ApplyUpdate(Employee employee, EmployeeUpdateDto dto, CancellationToken ct)
  {
    employee.FirstName = dto.Firstname;
    employee.LastName = dto.Lastname;
    employee.PhoneNumber = dto.PhoneNumber;

    if (!employee.Rol.Name.Equals(dto.Role))
    {
      var rol = rolRepository.FirstOrDefaultAsync(r => r.Name == dto.Role, ct).Result
        ?? throw new NotFoundException("Rol no encontrado");
      employee.Rol = rol;
    }

    if (dto.Email is not null && !dto.Email.Equals(employee.Email, StringComparison.OrdinalIgnoreCase))
    {
      // TODO: validate email and send email to the user to confirm the change.
    }
  }
}

public class ConcreteEmployeeUpdateAdmin(
    IEmployeeRepository employeeRepository,
    IRolRepository rolRepository
  ) : IEmployeeUpdateOneStrategy
{
  async public Task<Employee> ExecuteAsync(UserTokenDto user, int id, EmployeeUpdateDto dto, CancellationToken ct)
  {
    var employee = await employeeRepository.FindOne(e => e.Id == id, ct)
      ?? throw new NotFoundException("Empleado no encontrado");

    if (employee.Company.Id != user.CompanyId)
      throw new ForbiddenException("No puedes editar un empleado de otra empresa");

    await ApplyUpdateAsync(employee, dto, ct);

    await employeeRepository.SaveChangesAsync(ct);

    return employee;
  }

  private async Task ApplyUpdateAsync(Employee employee, EmployeeUpdateDto dto, CancellationToken ct)
  {
    employee.FirstName = dto.Firstname;
    employee.LastName = dto.Lastname;
    employee.PhoneNumber = dto.PhoneNumber;

    if (!employee.Rol.Name.Equals(dto.Role))
    {
      employee.Rol = await rolRepository.FirstOrDefaultAsync(r => r.Name == dto.Role, ct)
        ?? throw new NotFoundException("Rol no encontrado");
    }

    if (dto.Email is not null && !dto.Email.Equals(employee.Email, StringComparison.OrdinalIgnoreCase))
    {
      // TODO: validate email and send email to the user to confirm the change.
    }
  }
}

public class ConcreteEmployeeUpdateSuperAdmin(
    IEmployeeRepository employeeRepository,
    IRolRepository rolRepository
  ) : IEmployeeUpdateOneStrategy
{
  async public Task<Employee> ExecuteAsync(UserTokenDto user, int id, EmployeeUpdateDto dto, CancellationToken ct)
  {
    var employee = await employeeRepository.FindOne(e => e.Id == id, ct)
      ?? throw new NotFoundException("Empleado no encontrado");

    if (employee.Company.Id != user.CompanyId)
      throw new ForbiddenException("No puedes editar un empleado de otra empresa");

    await ApplyUpdateAsync(employee, dto, ct);

    await employeeRepository.SaveChangesAsync(ct);

    return employee;
  }

  private async Task ApplyUpdateAsync(Employee employee, EmployeeUpdateDto dto, CancellationToken ct)
  {
    employee.FirstName = dto.Firstname;
    employee.LastName = dto.Lastname;
    employee.PhoneNumber = dto.PhoneNumber;

    if (!employee.Rol.Name.Equals(dto.Role))
    {
      employee.Rol = await rolRepository.FirstOrDefaultAsync(r => r.Name == dto.Role, ct)
        ?? throw new NotFoundException("Rol no encontrado");
    }

    if (dto.Email is not null && !dto.Email.Equals(employee.Email, StringComparison.OrdinalIgnoreCase))
    {
      // TODO: validate email and send email to the user to confirm the change.
    }
  }
}
