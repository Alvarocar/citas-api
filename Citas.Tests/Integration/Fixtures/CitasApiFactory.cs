using Citas.Domain.Entities;
using Citas.Domain.Enums;
using Citas.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Citas.Tests.Integration.Fixtures;

/// <summary>
/// Starts a real postgres:18.0-alpine container once for the entire test collection,
/// runs EF migrations, seeds the Rol lookup table, and exposes a configured
/// WebApplicationFactory for integration tests.
/// </summary>
public class CitasApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private const string TestJwtKey = "TestKeyThatIsAtLeast32CharsLong!XYZ";
  private const string TestJwtIssuer = "https://test.citas";
  private const string TestJwtAudience = "https://test.citas";

  private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
      .WithImage("postgres:18.0-alpine")
      .WithDatabase("citasdb_test")
      .WithUsername("testuser")
      .WithPassword("testpass")
      .Build();

  public async Task InitializeAsync()
  {
    await _postgres.StartAsync();
    await RunMigrationsAndSeedAsync();
  }

  public new async Task DisposeAsync()
  {
    await _postgres.StopAsync();
    await base.DisposeAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Development");

    builder.ConfigureAppConfiguration((_, cfg) =>
    {
      cfg.AddInMemoryCollection(new Dictionary<string, string?>
      {
        ["ConnectionStrings:DBConnection"] = BuildConnectionString(),
        ["Jwt:Key"] = TestJwtKey,
        ["Jwt:Issuer"] = TestJwtIssuer,
        ["Jwt:Audiences:0"] = TestJwtAudience,
        ["Cors:Origins:0"] = TestJwtAudience,
      });
    });

    builder.ConfigureServices(services =>
    {
      // Replace the production NpgsqlDataSource and DbContext with ones that
      // point at the Testcontainers postgres instance. We build the data source
      // with MapEnum so Npgsql knows how to read/write the PG enum types, then
      // pass it directly to UseNpgsql (no o.MapEnum needed - that would duplicate).
      services.RemoveAll<NpgsqlDataSource>();
      services.RemoveAll<DbContextOptions<CitasDbContext>>();
      services.RemoveAll<DbContextOptions>();
      services.RemoveAll<CitasDbContext>();

      var dataSource = BuildTestDataSource();
      services.AddSingleton(dataSource);
      services.AddDbContext<CitasDbContext>(opts => opts
        .UseNpgsql(dataSource, o =>
        {
          o.MapEnum<EReservationState>("enum__reservation_state");
          o.MapEnum<EDay>("enum__day");
        })
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));
    });
  }

  /// <summary>
  /// Creates an HttpClient that automatically handles cookies so auth flows work.
  /// </summary>
  public HttpClient CreateClientWithCookies()
  {
    return CreateClient(new WebApplicationFactoryClientOptions
    {
      HandleCookies = true,
      AllowAutoRedirect = false,
    });
  }

  private string BuildConnectionString()
      => _postgres.GetConnectionString();

  private async Task RunMigrationsAndSeedAsync()
  {
    var dataSource = BuildTestDataSource();
    var optionsBuilder = new DbContextOptionsBuilder<CitasDbContext>();
    optionsBuilder
      .UseNpgsql(dataSource, o =>
      {
        o.MapEnum<EReservationState>("enum__reservation_state");
        o.MapEnum<EDay>("enum__day");
      })
      .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

    await using var context = new CitasDbContext(optionsBuilder.Options);
    await context.Database.MigrateAsync();
    await SeedRolesAsync(context);
    await ResetSequencesAsync(dataSource);
  }

  /// <summary>
  /// Migrations that explicitly insert rows with specific IDs (e.g. AddRoles, AddDefaultCompany)
  /// do not advance the IDENTITY sequences. This resets each affected sequence so subsequent
  /// INSERTs do not collide with the seeded rows.
  /// </summary>
  private static async Task ResetSequencesAsync(NpgsqlDataSource dataSource)
  {
    await using var conn = await dataSource.OpenConnectionAsync();

    await using var companyCmd = conn.CreateCommand();
    companyCmd.CommandText = "SELECT COALESCE(MAX(id), 0) + 1 FROM company";
    var companyNext = Convert.ToInt64(await companyCmd.ExecuteScalarAsync());
    await using var companyRestart = conn.CreateCommand();
    companyRestart.CommandText = $"ALTER TABLE company ALTER COLUMN id RESTART WITH {companyNext}";
    await companyRestart.ExecuteNonQueryAsync();

    await using var rolCmd = conn.CreateCommand();
    rolCmd.CommandText = "SELECT COALESCE(MAX(id), 0) + 1 FROM rol";
    var rolNext = Convert.ToInt64(await rolCmd.ExecuteScalarAsync());
    await using var rolRestart = conn.CreateCommand();
    rolRestart.CommandText = $"ALTER TABLE rol ALTER COLUMN id RESTART WITH {rolNext}";
    await rolRestart.ExecuteNonQueryAsync();
  }

  private NpgsqlDataSource BuildTestDataSource()
  {
    var builder = new NpgsqlDataSourceBuilder(BuildConnectionString());
    builder.MapEnum<EReservationState>("enum__reservation_state");
    builder.MapEnum<EDay>("enum__day");
    return builder.Build();
  }

  private static async Task SeedRolesAsync(CitasDbContext context)
  {
    if (await context.Rols.AnyAsync()) return;

    context.Rols.AddRange(
        new Rol { Id = Rol.AdministratorId, Name = Rol.Administrator },
        new Rol { Id = Rol.EmployeeId, Name = Rol.Employee },
        new Rol { Id = Rol.SuperAdministratorId, Name = Rol.SuperAdministrator }
    );

    await context.SaveChangesAsync();
  }
}

[CollectionDefinition(nameof(CitasApiCollection))]
public class CitasApiCollection : ICollectionFixture<CitasApiFactory> { }