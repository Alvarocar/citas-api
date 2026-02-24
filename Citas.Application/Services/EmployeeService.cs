using Citas.Application.Dto;
using Citas.Application.Employees.Strategies;
using Citas.Application.Factories;
using Citas.Domain.Entities;
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
    IReservationRepository _reservationRepository,
    IEmployeeScheduleExceptionRepository _employeeScheduleExceptionRepository,
    EmployeeCreateOneStrategy _employeeCreateOneStrategy,
    EmployeeGetByIdStrategy _employeeGetByIdStrategy,
    IEmployeeUpdateOneStrategyFactory _employeeUpdateOneStrategyFactory,
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

    var rol = await _rolRepository.FirstOrDefaultAsync(r => r.Id == Rol.SuperAdministratorId, ct);


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
    var strategy = _employeeCreateOneStrategy.GetStrategy(token);

    if (strategy is null) throw new ForbiddenException();

    var employee = await strategy.ExecuteAsync(dto, ct);

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

    var employees = await _employeeRepository.FindAllByCompanyId(currentUser, pagination, ct);

    return [.. employees.Select(e => new EmployeeOverviewDto
    {
      Id = e.Id,
      FirstName = e.FirstName,
      LastName = e.LastName,
      Email = e.Email,
      PhoneNumber = e.PhoneNumber ?? string.Empty,
      Role = e.Rol.Name,
    })];
  }

  async public Task<EmployeeOverviewDto?> GetById(int id, UserTokenDto token, CancellationToken ct)
  {

    var current = await _employeeRepository.GetByIdAsync(token.Id, ct);

    if (current is null) return null;

    var strategy = _employeeGetByIdStrategy.GetStrategy(token);

    if (strategy is null) return null;

    return await strategy.ExecuteAsync(id, ct);
  }

async public Task<EmployeeOverviewDto> Update(UserTokenDto token, EmployeeUpdateDto dto, int id, CancellationToken ct)
  {
    var strategy = _employeeUpdateOneStrategyFactory.GetStrategy(token);

    if (strategy is null) throw new ForbiddenException();

    var employee = await strategy.ExecuteAsync(token, id, dto, ct);

    return new EmployeeOverviewDto
    {
      Id = employee.Id,
      FirstName = employee.FirstName,
      LastName = employee.LastName,
      Email = employee.Email,
      PhoneNumber = employee.PhoneNumber ?? string.Empty,
      Role = employee.Rol.Name
    };
  }

  async public Task DeleteById(int id, UserTokenDto token, CancellationToken ct)
  {

    var current = await _employeeRepository.FirstOrDefaultWithIncludesAsync(
      e => e.Id == token.Id,
      ct,
      e => e.Company
    );

    if (current is null) throw new NotFoundException();

    var employee = await _employeeRepository.FirstOrDefaultWithIncludesAsync(e => e.Id == id && e.Company.Id == current.Company.Id, ct, e => e.Rol);

    if (employee is null) throw new NotFoundException("Empleado no encontrado");

    var reservations = await _reservationRepository.FindAsync(re => re.Employee.Id == employee.Id, ct);

    if (reservations is null || reservations.Any()) throw new ReservationAssignedException();


    if (token.Role == Rol.Administrator || token.Role == Rol.SuperAdministrator)
    {
      using (_unitOfWork)
      {
        await _unitOfWork.BeginTransactionAsync(ct);

        _employeeScheduleExceptionRepository.FindAsync(re => re.Employee.Id == id, ct)
          .ContinueWith(t =>
          {
            var exceptions = t.Result;
            foreach (var exception in exceptions)
            {
              _employeeScheduleExceptionRepository.Delete(exception);
            }
          }, ct).Wait(ct);

        _employeeRepository.Delete(employee);

        await _unitOfWork.SaveChangesAsync(ct);
        await _unitOfWork.CommitTransactionAsync(ct);
        return;
      }
    }
    if (token.Role == Rol.Employee && id == current.Id)
    {
      _employeeScheduleExceptionRepository.FindAsync(re => re.Employee.Id == current.Id, ct)
        .ContinueWith(t =>
        {
          var exceptions = t.Result;
          foreach (var exception in exceptions)
          {
            _employeeScheduleExceptionRepository.Delete(exception);
          }
        }, ct).Wait(ct);
      _employeeRepository.Delete(current);
      await _employeeRepository.SaveChangesAsync(ct);
      return;
    }
    throw new NotFoundException("Empleado no encontrado");
  }
}
