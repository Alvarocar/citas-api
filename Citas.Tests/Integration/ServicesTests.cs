using Citas.Application.Dto;
using Citas.Tests.Helpers;
using Citas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Citas.Tests.Integration;

[Collection(nameof(CitasApiCollection))]
public class ServicesTests
{
  private readonly CitasApiFactory _factory;

  public ServicesTests(CitasApiFactory factory)
  {
    _factory = factory;
  }

  // --- POST /api/services ---

  [Fact]
  public async Task Create_WithoutToken_Returns401()
  {
    var client = _factory.CreateClient();

    var response = await client.PostAsJsonAsync(TestConstants.Routes.Services, TestDtos.MakeServiceCreateDto());

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task Create_WithAdminToken_Returns201()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var response = await client.PostAsJsonAsync(TestConstants.Routes.Services, TestDtos.MakeServiceCreateDto());

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<ServiceOverviewDto>();
    Assert.NotNull(body);
    Assert.Equal("Test Service", body!.Name);
  }

  // --- GET /api/services ---

  [Fact]
  public async Task GetAll_WithoutToken_Returns401()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync(TestConstants.Routes.Services);

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task GetAll_WithToken_Returns200WithList()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();
    await client.PostAsJsonAsync(TestConstants.Routes.Services, TestDtos.MakeServiceCreateDto("Svc A"));
    await client.PostAsJsonAsync(TestConstants.Routes.Services, TestDtos.MakeServiceCreateDto("Svc B"));

    var response = await client.GetAsync(TestConstants.Routes.Services);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<List<ServiceOverviewDto>>();
    Assert.NotNull(body);
    Assert.True(body!.Count >= 2);
  }

  // --- GET /api/services/{id} ---

  [Fact]
  public async Task GetById_WithValidId_Returns200()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();
    var created = await CreateServiceAsync(client);

    var response = await client.GetAsync($"{TestConstants.Routes.Services}/{created.Id}");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<ServiceOverviewDto>();
    Assert.Equal(created.Id, body!.Id);
  }

  [Fact]
  public async Task GetById_WithUnknownId_Returns404()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var response = await client.GetAsync($"{TestConstants.Routes.Services}/{TestConstants.NotFoundId}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  // --- PUT /api/services/{id} ---

  [Fact]
  public async Task Update_WithValidData_Returns200()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();
    var created = await CreateServiceAsync(client);

    var updateDto = new ServiceUpdateDto
    {
      Name = "Updated Name",
      Description = "Updated Desc",
      SuggestedPrice = 50.0f,
      IsUnavailable = false
    };

    var response = await client.PutAsJsonAsync($"{TestConstants.Routes.Services}/{created.Id}", updateDto);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<ServiceOverviewDto>();
    Assert.Equal("Updated Name", body!.Name);
    Assert.Equal("Updated Desc", body.Description);
    Assert.Equal(50.0f, body.SuggestedPrice);
  }

  [Fact]
  public async Task Update_WithUnknownId_Returns404()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var updateDto = new ServiceUpdateDto
    {
      Name = "X",
      Description = "X",
      SuggestedPrice = 1.0f
    };

    var response = await client.PutAsJsonAsync($"{TestConstants.Routes.Services}/{TestConstants.NotFoundId}", updateDto);

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  // --- DELETE /api/services/{id} ---

  [Fact]
  public async Task Delete_WithValidId_Returns204()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();
    var created = await CreateServiceAsync(client);

    var response = await client.DeleteAsync($"{TestConstants.Routes.Services}/{created.Id}");

    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async Task Delete_WithAlreadyDeletedId_Returns404()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();
    var created = await CreateServiceAsync(client);

    await client.DeleteAsync($"{TestConstants.Routes.Services}/{created.Id}");
    var response = await client.DeleteAsync($"{TestConstants.Routes.Services}/{created.Id}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Fact]
  public async Task Delete_WithUnknownId_Returns404()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var response = await client.DeleteAsync($"{TestConstants.Routes.Services}/{TestConstants.NotFoundId}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  // --- Helpers ---

  private static async Task<ServiceOverviewDto> CreateServiceAsync(HttpClient client, string name = "Test Service")
  {
    var response = await client.PostAsJsonAsync(TestConstants.Routes.Services, TestDtos.MakeServiceCreateDto(name));
    response.EnsureSuccessStatusCode();
    return (await response.Content.ReadFromJsonAsync<ServiceOverviewDto>())!;
  }
}
