using Citas.Application.Employees.Strategies;
using Citas.Application.Factories;
using Citas.Application.Services;
using Citas.Infrastructure.Factories;
using Citas.Infrastructure.Factories.Employees;
using Citas.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Citas.Infrastructure.DependencyInjection;

public static class ServicesSetup
{
  public static IServiceCollection AddCitasServices(this IServiceCollection services)
  {
    // strategies
    services.AddScoped<EmployeeCreateOneStrategy, EmployeeCreateOneStrategy>();
    services.AddScoped<EmployeeGetByIdStrategy, EmployeeGetByIdStrategy>();
    services.AddScoped<IEmployeeUpdateOneStrategyFactory, EmployeeUpdateOneStrategyFactory>();

    // concrete strategies
    services.AddKeyedScoped<IEmployeeUpdateOneStrategy, ConcreteEmployeeUpdateSuperAdmin>("superadmin");
    services.AddKeyedScoped<IEmployeeUpdateOneStrategy, ConcreteEmployeeUpdateAdmin>("admin");
    services.AddKeyedScoped<IEmployeeUpdateOneStrategy, ConcreteEmployeeUpdateEmployee>("employee");

    // services
    services.AddScoped<EmployeeService, EmployeeService>();
    services.AddScoped<IPasswordHasherService, PasswordHasherService>();
    services.AddScoped<IJwtTokenService, JwtTokenService>();
    services.AddScoped<IServiceService, ServiceService>();

    // factories
    services.AddScoped<IServiceFactory, ServiceFactory>();
    return services;
  }
}
