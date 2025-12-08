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
  public async Task<IActionResult> GetMyProfile(CancellationToken ct, [FromQuery] PaginationFilter filters)
  {
    var user = GetUserTokenFromClaims();

    var employees = await _employeeService.GetAllEmployees(user, filters, ct);
    return Ok(employees);
  }
}
