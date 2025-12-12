using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Citas.Infrastructure.Security;

public static class CitasClaims
{
  public const string Id = JwtRegisteredClaimNames.Sub;
  public const string Email = JwtRegisteredClaimNames.Email;
  public const string FirstName = JwtRegisteredClaimNames.GivenName;
  public const string LastName = JwtRegisteredClaimNames.FamilyName;
  public const string Role = ClaimTypes.Role;
  public const string CompanyId = "company_id";
  public const string Audience = JwtRegisteredClaimNames.Aud;
}
