using Citas.Application.Dto;
using Citas.Application.Schedules.Strategies;
using Citas.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Citas.Infrastructure.Factories.Schedules;

internal class ScheduleCreateOneStrategyFactory(
    IServiceProvider serviceProvider
  ) : IScheduleCreateOneStrategyFactory
{
  public IScheduleCreateOneStrategy? GetStrategy(UserTokenDto user)
  {
    return user.Role switch
    {
      Rol.SuperAdministrator => serviceProvider.GetRequiredKeyedService<IScheduleCreateOneStrategy>("superadmin"),
      Rol.Administrator => serviceProvider.GetRequiredKeyedService<IScheduleCreateOneStrategy>("admin"),
      _ => null,
    };
  }
}
