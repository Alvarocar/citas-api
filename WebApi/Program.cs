using Citas.Infrastructure.DependencyInjection;
using Citas.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Middlewares;

internal class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddCitasInfrastructure(builder.Configuration);
    builder.Services.AddFactories();
    builder.Services.AddCitasServices();
    builder.Services.AddCitasCors(builder.Configuration);


    builder.Services.AddControllers(options =>
    {
      options.Filters.Add<CleanModelStateFilter>();
    })
    .AddJsonOptions(options =>
     {
       options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
       options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
     });

    builder.Services.AddSwaggerGen();

    builder.Services.AddCitasSecurity(builder.Configuration);

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
      options.SuppressModelStateInvalidFilter = true;
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
      var context = scope.ServiceProvider.GetRequiredService<CitasDbContext>();
      context.Database.Migrate();
    }

    // Configure the HTTP request pipeline.

    app.UseMiddleware<ErrorsMiddleware>();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseCors("AllowClient");

    app.Run();
  }
}