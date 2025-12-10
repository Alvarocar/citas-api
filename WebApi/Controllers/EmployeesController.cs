using Citas.Application.Dto;
using Citas.Application.Services;
using Citas.Domain.Enums;
using Citas.Domain.Filters;
using Citas.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(
    EmployeeService _employeeService,
    IJwtTokenService _jwtService,
    CookiesService _cookiesService
  ) : BaseController
{

  [HttpPost("create-admin")]
  public async Task<IActionResult> CreateAdmin([FromBody] EmployeeCreateAdminDto dto, CancellationToken ct)
  {
    var newUser = await _employeeService.CreateOneAdmin(dto, ct);
    var token = _jwtService.GenerateToken(newUser);
    _cookiesService.AppendTokenToCookies(Response, token);
    return Created(string.Empty, newUser);
  }

  [HttpPost]
  [Authorize(Roles = nameof(ERolType.ADMINISTRATOR))]
  public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto newEmployee, CancellationToken ct)
  {
    var user = GetUserTokenFromClaims();

    var createdEmployee = await _employeeService.CreateOne(newEmployee, user, ct);
    var token = _jwtService.GenerateToken(createdEmployee);
    _cookiesService.AppendTokenToCookies(Response, token);
    return Created(string.Empty, createdEmployee);
  }

  [HttpGet]
  [Authorize(Roles = nameof(ERolType.ADMINISTRATOR))]
  public async Task<IActionResult> GetAllEmployees(CancellationToken ct, [FromQuery] PaginationFilter filters)
  {
    var user = GetUserTokenFromClaims();

    var employees = await _employeeService.GetAllEmployees(user, filters, ct);
    return Ok(employees);
  }

  [HttpGet("{id}")]
  [Authorize(Roles = $"{nameof(ERolType.EMPLOYEE)},{nameof(ERolType.ADMINISTRATOR)}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var user = GetUserTokenFromClaims();
    var employee = await _employeeService.GetById(id, user, ct);

    if (employee is null)
    {
      return NotFound();
    }

    return Ok(employee);
  }

  /// <summary>
  ///   Remove the employee with given id.
  ///   For now this endpoint just can be used by a user with administrator role.
  /// </summary>
  /// <response code="204">The employee was deleted</response>
  /// <respose code="404">The employee with given id was not found or is not in the same company</respose>
  /// <response code="409">The employee can not be deleted because has associated records that prevent its deletion</response>
  /// 
  [HttpDelete("{id}")]
  [Authorize(Roles = nameof(ERolType.ADMINISTRATOR))]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var user = GetUserTokenFromClaims();
    await _employeeService.DeleteById(id, user, ct);
    return NoContent();
  }
}
