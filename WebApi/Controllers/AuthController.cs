using Citas.Application.Dto;
using Citas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    EmployeeService _employeeService,
    IJwtTokenService _jwtService
  ) : BaseController
{
  [HttpPut("login")]
  public async Task<IActionResult> SignIn([FromBody] EmployeeSignInDto employee, CancellationToken ct)
  {
    var data = await _employeeService.SignIn(employee, ct);
    var token = _jwtService.GenerateToken(data);
    this.AppendTokenToCookies(token);
    return Accepted(string.Empty, data);
  }
}
