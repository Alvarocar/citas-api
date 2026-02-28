using Citas.Application.Dto;
using Citas.Tests.Helpers;
using Citas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Citas.Tests.Integration;

[Collection(nameof(CitasApiCollection))]
public class SchedulesTests
{
  private readonly CitasApiFactory _factory;

  public SchedulesTests(CitasApiFactory factory)
  {
    _factory = factory;
  }

  // --- POST /api/schedules ---

  [Fact]
  public async Task Create_WithoutToken_Returns401()
  {
    var client = _factory.CreateClient();

    var response = await client.PostAsJsonAsync(TestConstants.Routes.Schedules, TestDtos.MakeScheduleCreateDto());

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task Create_WithAdminToken_Returns201WithBody()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var dto = TestDtos.MakeScheduleCreateDto("Evening Shift");
    var response = await client.PostAsJsonAsync(TestConstants.Routes.Schedules, dto);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<EmployeeScheduleDto>(TestConstants.JsonOptions);
    Assert.NotNull(body);
    Assert.True(body!.Id > 0);
    Assert.Equal("Evening Shift", body.Name);
    Assert.Equal(dto.Ranges.Count, body.Ranges.Count);
  }

  [Fact]
  public async Task Create_WithInvalidDto_Returns400()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var invalidDto = new { Ranges = Array.Empty<object>() };
    var response = await client.PostAsJsonAsync(TestConstants.Routes.Schedules, invalidDto);

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }
}
