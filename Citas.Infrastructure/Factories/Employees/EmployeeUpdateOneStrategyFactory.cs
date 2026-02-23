using Citas.Application.Dto;
using Citas.Application.Employees.Strategies;
using Citas.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Citas.Infrastructure.Factories.Employees;

public class EmployeeUpdateOneStrategyFactory(IServiceProvider serviceProvider) : IEmployeeUpdateOneStrategyFactory
{
  public IEmployeeUpdateOneStrategy? GetStrategy(UserTokenDto user)
  {
    return user.Role switch
    {
      Rol.SuperAdministrator => serviceProvider.GetRequiredKeyedService<IEmployeeUpdateOneStrategy>("superadmin"),
      Rol.Administrator => serviceProvider.GetRequiredKeyedService<IEmployeeUpdateOneStrategy>("admin"),
      Rol.Employee => serviceProvider.GetRequiredKeyedService<IEmployeeUpdateOneStrategy>("employee"),
      _ => null,
    };
  }
}
