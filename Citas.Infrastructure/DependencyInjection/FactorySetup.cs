using Microsoft.Extensions.DependencyInjection;

namespace Citas.Infrastructure.DependencyInjection;

public static class FactorySetup
{
  public static IServiceCollection AddFactories(this IServiceCollection services)
  {
    services.AddScoped<Application.Factories.IEmployeeFactory, Factories.EmployeeFactory>();
    return services;
  }
}
