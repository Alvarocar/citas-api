using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Citas.Infrastructure.DependencyInjection;

public static class CorsSetup
{
  public static IServiceCollection AddCitasCors(this IServiceCollection services, IConfiguration configuration)
  {
    var origins = configuration.GetSection("Cors:Origins").Get<string[]>();

    if (origins == null) return services;

    services.AddCors(options =>
    {
      options.AddPolicy("AllowClient", policy =>
      {
        policy.WithOrigins(origins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
      });
    });
    return services;
  }
}
