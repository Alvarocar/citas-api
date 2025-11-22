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

  private void _appendTokenToCookies(string token)
  {
    Response.Cookies.Append(
      "access_token",
      token,
      new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        Expires = DateTime.UtcNow.AddHours(24),
        SameSite = SameSiteMode.None,
      }
     );
  }

  [HttpPost("create-admin")]
  public Task CreateAdmin()
  {

    return Task.CompletedTask;
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
    this._appendTokenToCookies(token);
    return Created(string.Empty, createdEmployee);
  }

  [HttpPut("sign-in")]
  public async Task<IActionResult> SignIn([FromBody] EmployeeSignInDto employee, CancellationToken ct)
  {
    var data = await _employeeService.SignIn(employee, ct);
    var token = _jwtService.GenerateToken(data);
    this._appendTokenToCookies(token);
    return Accepted(string.Empty, data);
  }
}
