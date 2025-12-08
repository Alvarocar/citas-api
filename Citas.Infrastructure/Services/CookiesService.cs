using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Citas.Infrastructure.Services;

public class CookiesService
{
  private readonly IHostEnvironment _hostEnvironment;
  private readonly bool _isDevelopment;

  public CookiesService(IHostEnvironment hostEnvironment)
  {
    _hostEnvironment = hostEnvironment;
    _isDevelopment = _hostEnvironment.IsDevelopment();
  }

  public void AppendTokenToCookies(HttpResponse response, string token)
  {
    response.Cookies.Append(
        "access_token",
        token,
        new CookieOptions
        {
          HttpOnly = true,
          Secure = !_isDevelopment,
          Expires = DateTime.UtcNow.AddDays(2),
          SameSite = _isDevelopment ? SameSiteMode.Lax : SameSiteMode.None,
        }
    );
  }
}
