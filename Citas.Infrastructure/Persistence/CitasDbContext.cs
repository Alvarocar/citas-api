using Citas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Citas.Infrastructure.Persistence;

public class CitasDbContext : DbContext
{
  public CitasDbContext(DbContextOptions<CitasDbContext> options) : base(options) { }

  public DbSet<Company> Companies { get; set; } = null!;
  public DbSet<Employee> Employees { get; set; } = null!;
  public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; } = null!;
  public DbSet<EmployeeScheduleException> EmployeeScheduleExceptions { get; set; } = null!;
  public DbSet<EmployeeService> EmployeeServices { get; set; } = null!;
  public DbSet<EmployeeScheduleRange> EmployeeScheduleRanges { get; set; } = null!;
  public DbSet<Position> Positions { get; set; } = null!;
  public DbSet<Rol> Rols { get; set; } = null!;
  public DbSet<Service> Services { get; set; } = null!;
  public DbSet<Reservation> Reservations { get; set; } = null!;
  public DbSet<Client> Clients { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // Apply all configurations from the current project (infrastructure).
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(CitasDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}