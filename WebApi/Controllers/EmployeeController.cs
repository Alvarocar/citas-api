using Citas.Application.Dto;
using Citas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(
    EmployeeService _employeeService,
    IJwtTokenService _jwtService
  ) : ControllerBase
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

  [HttpPost]
  public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto newEmployee, CancellationToken ct)
  {
    var createdEmployee = await _employeeService.CreateOne(newEmployee, ct);
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
