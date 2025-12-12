using Citas.Application.Dto;
using Citas.Application.Factories;
using Citas.Application.Services;
using Citas.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Citas.Infrastructure.Services;

public class JwtTokenService(
  IConfiguration _configuration,
  IEmployeeFactory _employeeFactory
  ) : IJwtTokenService
{
  public string GenerateToken(UserTokenDto user)
  {
    var audiences = _configuration.GetSection("Jwt:Audiences").Get<string[]>();

    var claims = new List<Claim>
    {
      new(CitasClaims.Id, user.Id.ToString()),
      new(CitasClaims.Email, user.Email ?? ""),
      new(CitasClaims.FirstName, user.FirstName),
      new(CitasClaims.LastName, user.LastName),
      new(CitasClaims.Role, user.Role),
      new(CitasClaims.CompanyId, user.CompanyId.ToString())
    };

    if (audiences != null)
    {
      claims.AddRange(audiences.Select(aud => new Claim(CitasClaims.Audience, aud)));
    }

    var descriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddHours(24),
      SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        ),
        SecurityAlgorithms.HmacSha256
      ),
      Issuer = _configuration["Jwt:Issuer"],
    };

    var tokenHandler = new JsonWebTokenHandler();

    return tokenHandler.CreateToken(descriptor);
  }

  public UserTokenDto? GetUserToken(string token)
  {
    return _employeeFactory.CreateToken(token);
  }
}
