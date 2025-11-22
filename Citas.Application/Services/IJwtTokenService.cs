using Citas.Application.Dto;

namespace Citas.Application.Services;

public interface IJwtTokenService
{
  string GenerateToken(UserTokenDto user);

  UserTokenDto? GetUserToken(string token);
}
