using Citas.Application.Dto;
using Citas.Tests.Helpers;
using Citas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Citas.Tests.Integration;

[Collection(nameof(CitasApiCollection))]
public class AuthTests
{
  private readonly CitasApiFactory _factory;

  private static readonly string AdminEmail = $"auth_admin_{Guid.NewGuid():N}@test.com";

  public AuthTests(CitasApiFactory factory)
  {
    _factory = factory;
    EnsureAdminCreated().GetAwaiter().GetResult();
  }

  private async Task EnsureAdminCreated()
  {
    var client = _factory.CreateClientWithCookies();
    var dto = TestDtos.MakeCreateAdminDto();
    dto.Email = AdminEmail;
    await client.PostAsJsonAsync(TestConstants.Routes.CreateAdmin, dto);
  }

  [Fact]
  public async Task Login_WithValidCredentials_Returns202AndSetsCookie()
  {
    var client = _factory.CreateClientWithCookies();

    var response = await client.PutAsJsonAsync(TestConstants.Routes.Login, new EmployeeSignInDto
    {
      Email = AdminEmail,
      Password = TestConstants.AdminPassword
    });

    Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

    var cookies = response.Headers.GetValues("Set-Cookie");
    Assert.Contains(cookies, c => c.StartsWith("access_token="));
  }

  [Fact]
  public async Task Login_WithInvalidPassword_Returns404()
  {
    var client = _factory.CreateClientWithCookies();

    var response = await client.PutAsJsonAsync(TestConstants.Routes.Login, new EmployeeSignInDto
    {
      Email = AdminEmail,
      Password = "WrongPassword!"
    });

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact]
  public async Task Login_WithUnknownEmail_Returns404()
  {
    var client = _factory.CreateClientWithCookies();

    var response = await client.PutAsJsonAsync(TestConstants.Routes.Login, new EmployeeSignInDto
    {
      Email = "nobody@nowhere.com",
      Password = "SomePass"
    });

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact]
  public async Task Check_WithValidCookie_Returns202()
  {
    var client = _factory.CreateClientWithCookies();

    await client.PutAsJsonAsync(TestConstants.Routes.Login, new EmployeeSignInDto
    {
      Email = AdminEmail,
      Password = TestConstants.AdminPassword
    });

    var checkResponse = await client.GetAsync(TestConstants.Routes.Check);

    Assert.Equal(HttpStatusCode.Accepted, checkResponse.StatusCode);
  }

  [Fact]
  public async Task Check_WithoutCookie_Returns401()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync(TestConstants.Routes.Check);

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task Logout_Returns204()
  {
    var client = _factory.CreateClientWithCookies();

    var response = await client.DeleteAsync(TestConstants.Routes.Logout);

    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }
}
