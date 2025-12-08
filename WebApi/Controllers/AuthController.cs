using Citas.Application.Dto;
using Citas.Application.Services;
using Citas.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    EmployeeService _employeeService,
    IJwtTokenService _jwtService,
    CookiesService _cookiesService
  ) : BaseController
{
  [HttpPut("login")]
  public async Task<IActionResult> SignIn([FromBody] EmployeeSignInDto employee, CancellationToken ct)
  {
    var data = await _employeeService.SignIn(employee, ct);
    var token = _jwtService.GenerateToken(data);
    _cookiesService.AppendTokenToCookies(Response, token);
    return Accepted(string.Empty, data);
  }

  [HttpGet("check")]
  [Authorize]
  public IActionResult Check()
  {
    var user = GetUserTokenFromClaims();
    return Accepted(string.Empty, user);
  }
}
