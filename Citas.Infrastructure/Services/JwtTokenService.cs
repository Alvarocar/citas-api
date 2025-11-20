using Citas.Application.Dto;
using Citas.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Citas.Infrastructure.Services
{
  public class JwtTokenService : IJwtTokenService
  {
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GenerateToken(UserTokenDto user)
    {
      var audiences = _configuration.GetSection("Jwt:Audiences").Get<string[]>();

      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
        new Claim(ClaimTypes.Role, user.Rol.ToString())
      };

      if (audiences != null)
      {
        claims.AddRange(audiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));
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
  }
}
