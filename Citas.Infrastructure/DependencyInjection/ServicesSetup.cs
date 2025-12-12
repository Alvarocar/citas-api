using Citas.Application.Factories;
using Citas.Application.Services;
using Citas.Infrastructure.Factories;
using Citas.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Citas.Infrastructure.DependencyInjection;

public static class ServicesSetup
{
  public static IServiceCollection AddCitasServices(this IServiceCollection services)
  {
    services.AddScoped<EmployeeService, EmployeeService>();
    services.AddScoped<IPasswordHasherService, PasswordHasherService>();
    services.AddScoped<IJwtTokenService, JwtTokenService>();
    services.AddScoped<IServiceService, ServiceService>();
    services.AddScoped<IServiceFactory, ServiceFactory>();
    return services;
  }
}
