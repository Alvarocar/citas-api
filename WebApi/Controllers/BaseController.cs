using Citas.Application.Dto;
using Citas.Domain.Exceptions;
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
      var id = User.FindFirst(ClaimTypes.NameIdentifier);
      if (id == null) throw new NotAuthorizedException();
      var role = User.FindFirst(ClaimTypes.Role);
      if (role == null) throw new NotAuthorizedException();
      var LastName = User.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
      return new UserTokenDto
      {
        Id = int.Parse(id.Value),
        Email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
        FirstName = User.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
        LastName = User.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty,
        Role = role.Value ?? string.Empty,
      };
    }
    catch
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