using Citas.Application.Services;
using Citas.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Citas.Infrastructure.DependencyInjection;

public static class SecuritySetup
{
  public static IServiceCollection AddCitasSecurity(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddScoped<IJwtTokenService, JwtTokenService>();

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
       options.TokenValidationParameters = new TokenValidationParameters
       {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = configuration["Jwt:Issuer"],
         ValidAudiences = configuration.GetSection("Jwt:Audiences").Get<string[]>(),
         IssuerSigningKey = new SymmetricSecurityKey(
           Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
          )
       };

       options.Events = new JwtBearerEvents
       {
         OnMessageReceived = context =>
         {
           if (context.Request.Cookies.ContainsKey("access_token"))
           {
             context.Token = context.Request.Cookies["access_token"];
           }
           return Task.CompletedTask;
         }
       };
     });

    return services;
  }
}
