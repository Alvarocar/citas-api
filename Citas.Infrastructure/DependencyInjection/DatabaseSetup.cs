namespace Citas.Infrastructure.DependencyInjection;
using Citas.Domain.Enums;
using Citas.Domain.Repositories;
using Citas.Infrastructure.Persistence;
using Citas.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class DatabaseSetup
{
  public static IServiceCollection AddCitasInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    var conn = configuration.GetConnectionString("DBConnection");
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(conn);
    dataSourceBuilder.MapEnum<EReservationState>("enum__reservation_state");
    dataSourceBuilder.MapEnum<EDay>("enum__day");
    var dataSource = dataSourceBuilder.Build();
    services.AddSingleton(dataSource);
    services.AddDbContext<CitasDbContext>(opts => opts
      .UseNpgsql(dataSource));

    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Repositorios concretos
    services.AddScoped<IReservationRepository, ReservationRepository>();
    services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    services.AddScoped<IClientRepository, ClientRepository>();
    services.AddScoped<ICompanyRepository, CompanyRepository>();
    services.AddScoped<IEmployeeScheduleRepository, EmployeeScheduleRepository>();
    services.AddScoped<IEmployeeScheduleRangeRepository, EmployeeScheduleRangeRepository>();
    services.AddScoped<IEmployeeScheduleExceptionRepository, EmployeeScheduleExceptionRepository>();
    services.AddScoped<IEmployeeServiceRepository, EmployeeServiceRepository>();
    services.AddScoped<IServiceRepository, ServiceRepository>();
    services.AddScoped<IRolRepository, RolRepository>();
    services.AddScoped<IPositionRepository, PositionRepository>();

    return services;
  }
}
