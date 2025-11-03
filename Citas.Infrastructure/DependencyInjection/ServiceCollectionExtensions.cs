using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Citas.Domain.Repositories;
using Citas.Infrastructure.Persistence;
using Citas.Infrastructure.Persistence.Repositories;

namespace Citas.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCitasInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CitasDbContext>(opts => opts.UseNpgsql(conn));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositorios concretos
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEmployeeScheduleRepository, EmployeeScheduleRepository>();
        services.AddScoped<IEmployeeScheduleExceptionRepository, EmployeeScheduleExceptionRepository>();
        services.AddScoped<IEmployeeServiceRepository, EmployeeServiceRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();

        return services;
    }
}