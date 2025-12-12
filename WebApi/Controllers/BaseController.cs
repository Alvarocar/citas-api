using Citas.Application.Dto;
using Citas.Domain.Exceptions;
using Citas.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

public abstract class BaseController : ControllerBase
{

  protected UserTokenDto GetUserTokenFromClaims()
  {
    if (User?.Identity?.IsAuthenticated != true) throw new NotAuthorizedException();

    try
    {

      var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);

      if (!claims.TryGetValue(ClaimTypes.NameIdentifier, out var id) ||
        !claims.TryGetValue(CitasClaims.Role, out var role))
      {
        throw new NotAuthorizedException();
      }

      return new UserTokenDto
      {
        Id = int.Parse(id),
        Email = claims.TryGetValue(ClaimTypes.Email, out var email) ? email : string.Empty,
        FirstName = claims.TryGetValue(ClaimTypes.GivenName, out var firstName) ? firstName : string.Empty,
        LastName = claims.TryGetValue(ClaimTypes.Surname, out var lastName) ? lastName : string.Empty,
        Role = role ?? string.Empty,
        CompanyId = int.Parse(User.FindFirst(CitasClaims.CompanyId)?.Value ?? "-1"),
      };
    }
    catch (Exception e)
    {
      throw new NotAuthorizedException();
    }
  }

  protected bool TryToGetUserTokenFromClaims(out UserTokenDto? user)
  {
    try
    {
      user = GetUserTokenFromClaims();
      return true;
    }
    catch
    {
      user = null;
      return false;
    }
  }
}