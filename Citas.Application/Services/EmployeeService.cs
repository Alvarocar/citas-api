using Citas.Application.Dto;
using Citas.Domain.Entities;
using Citas.Domain.Enums;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;

namespace Citas.Application.Services;

public class EmployeeService(
    IEmployeeRepository _employeeRepository,
    IRolRepository _rolRepository,
    ICompanyRepository _companyRepository,
    IPasswordHasherService _passwordHasher
  )
{

  async public Task<UserTokenDto> CreateOne(EmployeeCreateDto dto, CancellationToken ct)
  {

    var email = dto.Email.ToLower();

    var found = await _employeeRepository.FindAsync(
       employee => employee.Email == email,
       ct
     );

    if (found != null)
    {
      throw new AlreadyExistException(Resources.Errors.EmployeeEmailAlreadyExists);
    }

    var hashedPassword = _passwordHasher.HashPassword(dto.Password);

    var rol = await _rolRepository.FirstOrDefaultAsync(r => r.Type == ERolType.ADMINISTRATOR, ct);


    if (rol == null)
    {
      throw new CitasInternalException();
    }

    var company = await _companyRepository.GetByIdAsync(dto.CompanyId, ct);

    if (company == null)
    {
      throw new NotFoundException(Resources.Errors.CompanyNotFound);
    }

    _employeeRepository.AttachRol(rol);
    _employeeRepository.AttachCompany(company);

    var newEmployee = new Employee
    {
      FirstName = dto.Firstname,
      LastName = dto.Lastname,
      Email = email,
      Password = hashedPassword,
      IsActive = true,
      HasAccount = true,
      PhoneNumber = "",
      Rol = rol,
      Company = company,
    };

    _employeeRepository.Add(newEmployee);

    await _employeeRepository.SaveChangesAsync();

    return new UserTokenDto
    {
      Id = newEmployee!.Id,
      FirstName = newEmployee.FirstName,
      LastName = newEmployee.LastName,
      Email = newEmployee.Email!,
      Rol = newEmployee.Rol.Type,
    };

  }

  async public Task<UserTokenDto> SignIn(EmployeeSignInDto signIn, CancellationToken ct)
  {
    var employee = await _employeeRepository.FirstOrDefaultWithIncludesAsync(
      e => e.Email == signIn.Email.ToLower(),
      ct,
      e => e.Rol
      );
    if (employee == null)
    {
      throw new NotFoundException(Resources.Errors.SignIn);
    }

    if (!_passwordHasher.VerifyHashedPassword(employee.Password!, signIn.Password))
    {
      throw new NotFoundException(Resources.Errors.SignIn);
    }

    return new UserTokenDto
    {
      Id = employee.Id,
      FirstName = employee.FirstName,
      LastName = employee.LastName,
      Email = employee.Email!,
      Rol = employee.Rol.Type,
    };
  }
}
