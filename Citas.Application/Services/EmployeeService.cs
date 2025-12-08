using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Domain.Entities;
using Citas.Domain.Enums;
using Citas.Domain.Exceptions;
using Citas.Domain.Filters;
using Citas.Domain.Repositories;

namespace Citas.Application.Services;

public class EmployeeService(
    IEmployeeRepository _employeeRepository,
    IRolRepository _rolRepository,
    ICompanyRepository _companyRepository,
    IPasswordHasherService _passwordHasher,
    IEmployeeFactory _employeeFactory,
    IUnitOfWork _unitOfWork
  )
{

  async public Task<UserTokenDto> CreateOneAdmin(EmployeeCreateAdminDto dto, CancellationToken ct)
  {

    if ((await _employeeRepository.FindByEmail(dto.Email, ct)) is not null)
    {
      throw new AlreadyExistException(Resources.Errors.EmployeeEmailAlreadyExists);
    }

    if ((await _companyRepository.FirstOrDefaultAsync(com => com.Email == dto.Email.ToLower())) is not null)
    {
      throw new AlreadyExistException(Resources.Errors.EmployeeEmailAlreadyExists);
    }

    var rol = await _rolRepository.FirstOrDefaultAsync(r => r.Type == ERolType.ADMINISTRATOR, ct);


    if (rol == null)
    {
      throw new CitasInternalException();
    }

    using (_unitOfWork)
    {
      await _unitOfWork.BeginTransactionAsync(ct);
      var newCompany = new Company()
      {
        Name = dto.Company.Name,
        Address = dto.Company.Address,
        Email = dto.Company.Email.ToLower(),
        PhoneNumber = dto.Company.PhoneNumber,
      };
      _companyRepository.Add(newCompany);
      _employeeRepository.AttachRol(rol);
      var newEmployee = _employeeFactory.CreateAdmin(dto, rol, newCompany);
      _employeeRepository.Add(newEmployee);

      await _unitOfWork.SaveChangesAsync(ct);
      await _unitOfWork.CommitTransactionAsync(ct);

      return _employeeFactory.CreateToken(newEmployee);
    }
  }

  async public Task<UserTokenDto> CreateOne(EmployeeCreateDto dto, UserTokenDto token, CancellationToken ct)
  {
    var rol = await _rolRepository.FirstOrDefaultAsync(r => r.Type == dto.RoleType, ct);

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

  async public Task<List<EmployeeOverviewDto>> GetAllEmployees(UserTokenDto dto, PaginationFilter pagination, CancellationToken ct)
  {
    var currentUser = await _employeeRepository.FindById(dto.Id, ct);

    if (currentUser == null)
    {
      throw new NotFoundException();
    }

    var employees = await _employeeRepository.FindAllByCompanyId(currentUser.Company.Id, pagination, ct);

    return [.. employees.Select(e => new EmployeeOverviewDto
    {
      Id = e.Id,
      FirstName = e.FirstName,
      LastName = e.LastName,
      Email = e.Email,
      PhoneNumber = e.PhoneNumber ?? string.Empty,
      Role = e.Rol.Type.ToString()
    })];
  }
}
