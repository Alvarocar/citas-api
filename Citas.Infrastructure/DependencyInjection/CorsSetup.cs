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
      options.AddPolicy("CorsPolicy", policy =>
      {
        policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(origins);
      });
    });
    return services;
  }
}
