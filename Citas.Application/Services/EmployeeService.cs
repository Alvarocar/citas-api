using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Domain.Enums;
using Citas.Domain.Exceptions;
using Citas.Domain.Repositories;

namespace Citas.Application.Services;

public class EmployeeService(
    IEmployeeRepository _employeeRepository,
    IRolRepository _rolRepository,
    ICompanyRepository _companyRepository,
    IPasswordHasherService _passwordHasher,
    IEmployeeFactory _employeeFactory
  )
{

  async public Task<UserTokenDto> CreateOneAdmin(EmployeeCreateAdminDto dto, CancellationToken ct)
  {

    if ((await _employeeRepository.FindByEmail(dto.Email, ct)) != null)
    {
      throw new AlreadyExistException(Resources.Errors.EmployeeEmailAlreadyExists);
    }

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

    var newEmployee = _employeeFactory.CreateAdmin(dto, rol, company);

    _employeeRepository.Add(newEmployee);

    await _employeeRepository.SaveChangesAsync();

    return _employeeFactory.CreateToken(newEmployee);

  }

  async public Task<UserTokenDto> CreateOne(EmployeeCreateDto dto, UserTokenDto token, CancellationToken ct)
  {
    var rol = await _rolRepository.FirstOrDefaultAsync(r => r.Type == dto.RolType, ct);

    if (rol == null)
    {
      throw new CitasInternalException();
    }

    var currentUser = await _employeeRepository.FirstOrDefaultWithIncludesAsync(
      e => e.Id == token.Id,
      ct,
      e => e.Company
    );

    _employeeRepository.AttachRol(rol);
    _employeeRepository.AttachCompany(currentUser!.Company);

    var employee = _employeeFactory.Create(dto, rol, currentUser.Company);

    _employeeRepository.Add(employee);

    await _employeeRepository.SaveChangesAsync();

    return _employeeFactory.CreateToken(employee);
  }

  async public Task<UserTokenDto> SignIn(EmployeeSignInDto signIn, CancellationToken ct)
  {
    var employee = await _employeeRepository.FindByEmail(signIn.Email.ToLower(), ct);

    if (employee == null)
    {
      throw new NotFoundException(Resources.Errors.SignIn);
    }

    if (!_passwordHasher.VerifyHashedPassword(employee.Password!, signIn.Password))
    {
      throw new NotFoundException(Resources.Errors.SignIn);
    }

    return _employeeFactory.CreateToken(employee);
  }
}
