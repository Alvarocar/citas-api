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
  /// <summary>
  ///   authenticate employees and create session
  /// </summary>
  /// <response code="202">Returns the session data</response>
  /// <response code="404">Credentials are invalid</response>
  [HttpPut("login")]
  public async Task<IActionResult> SignIn([FromBody] EmployeeSignInDto employee, CancellationToken ct)
  {
    var data = await _employeeService.SignIn(employee, ct);
    var token = _jwtService.GenerateToken(data);
    _cookiesService.AppendTokenToCookies(Response, token);
    return Accepted(string.Empty, data);
  }
  /// <summary>
  ///   Check if the session is valid and return session object
  /// </summary>
  /// <response code="202">Returns the session data</response>
  [HttpGet("check")]
  [Authorize]
  public IActionResult Check()
  {
    var user = GetUserTokenFromClaims();
    return Accepted(string.Empty, user);
  }

  /// <summary>
  ///   Remove cookies associated with authentication
  /// </summary>
  /// <response code="204">Logout was successful</response>
  [HttpDelete("logout")]
  public IActionResult RemoveSession()
  {
    _cookiesService.DeleteTokenFromCookies(Response);
    return NoContent();
  }
}
