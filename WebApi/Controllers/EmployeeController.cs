using Citas.Application.Dto;
using Citas.Application.Services;
using Citas.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(
    EmployeeService _employeeService,
    IJwtTokenService _jwtService
  ) : BaseController
{

  [HttpPost("create-admin")]
  public async Task<IActionResult> CreateAdmin([FromBody] EmployeeCreateAdminDto dto, CancellationToken ct)
  {
    var newUser = await _employeeService.CreateOneAdmin(dto, ct);
    var token = _jwtService.GenerateToken(newUser);
    AppendTokenToCookies(token);
    return Created(string.Empty, newUser);
  }

  [HttpPost]
  [Authorize(Roles = nameof(ERolType.ADMINISTRATOR))]
  public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto newEmployee, CancellationToken ct)
  {
    var user = GetUserTokenFromClaims();
    if (user == null)
    {
      return Unauthorized();
    }

    var createdEmployee = await _employeeService.CreateOne(newEmployee, user, ct);
    var token = _jwtService.GenerateToken(createdEmployee);
    AppendTokenToCookies(token);
    return Created(string.Empty, createdEmployee);
  }
}
