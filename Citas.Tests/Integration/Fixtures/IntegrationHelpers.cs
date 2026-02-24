using Citas.Application.Dto;
using Citas.Tests.Helpers;
using System.Net.Http.Json;

namespace Citas.Tests.Integration.Fixtures;

public static class CitasApiFactoryExtensions
{
  /// <summary>
  /// Creates a cookie-carrying HttpClient, registers a fresh SuperAdmin with a
  /// GUID-unique email, and returns both the client and the deserialized token.
  /// </summary>
  public static async Task<(HttpClient client, UserTokenDto token)>
      CreateAuthenticatedClientAndTokenAsync(this CitasApiFactory factory)
  {
    var client = factory.CreateClientWithCookies();
    var dto = TestDtos.MakeCreateAdminDto();
    var response = await client.PostAsJsonAsync(TestConstants.Routes.CreateAdmin, dto);
    var token = await response.Content.ReadFromJsonAsync<UserTokenDto>();
    return (client, token!);
  }

  /// <summary>
  /// Convenience overload that discards the token when only the authenticated
  /// client is needed.
  /// </summary>
  public static async Task<HttpClient>
      CreateAuthenticatedClientAsync(this CitasApiFactory factory)
  {
    var (client, _) = await factory.CreateAuthenticatedClientAndTokenAsync();
    return client;
  }
}
