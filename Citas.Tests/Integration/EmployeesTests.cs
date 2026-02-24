using Citas.Application.Dto;
using Citas.Tests.Helpers;
using Citas.Tests.Integration.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Citas.Tests.Integration;

[Collection(nameof(CitasApiCollection))]
public class EmployeesTests
{
  private readonly CitasApiFactory _factory;

  public EmployeesTests(CitasApiFactory factory)
  {
    _factory = factory;
  }

  // --- create-admin ---

  [Fact]
  public async Task CreateAdmin_WithValidDto_Returns201AndSetsCookie()
  {
    var client = _factory.CreateClientWithCookies();
    var dto = TestDtos.MakeCreateAdminDto();

    var response = await client.PostAsJsonAsync(TestConstants.Routes.CreateAdmin, dto);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    var cookies = response.Headers.GetValues("Set-Cookie");
    Assert.Contains(cookies, c => c.StartsWith("access_token="));
  }

  [Fact]
  public async Task CreateAdmin_WithDuplicateEmail_Returns409()
  {
    var client = _factory.CreateClientWithCookies();
    var dto = TestDtos.MakeCreateAdminDto(emailSuffix: "dup");

    await client.PostAsJsonAsync(TestConstants.Routes.CreateAdmin, dto);
    var response = await client.PostAsJsonAsync(TestConstants.Routes.CreateAdmin, dto);

    Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
  }

  // --- GET /api/employees ---

  [Fact]
  public async Task GetAll_WithoutToken_Returns401()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync(TestConstants.Routes.Employees);

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task GetAll_WithAdminToken_Returns200WithList()
  {
    var client = await _factory.CreateAuthenticatedClientAsync();

    var response = await client.GetAsync(TestConstants.Routes.Employees);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<List<EmployeeOverviewDto>>();
    Assert.NotNull(body);
  }

  // --- GET /api/employees/{id} ---

  [Fact]
  public async Task GetById_WithValidId_Returns200()
  {
    var (client, token) = await _factory.CreateAuthenticatedClientAndTokenAsync();

    var response = await client.GetAsync($"{TestConstants.Routes.Employees}/{token.Id}");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task GetById_WithUnknownId_Returns404()
  {
    var (client, _) = await _factory.CreateAuthenticatedClientAndTokenAsync();

    var response = await client.GetAsync($"{TestConstants.Routes.Employees}/{TestConstants.NotFoundId}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  // --- PUT /api/employees/{id} ---

  [Fact]
  public async Task Update_WithValidData_Returns200()
  {
    var (client, token) = await _factory.CreateAuthenticatedClientAndTokenAsync();

    var dto = new EmployeeUpdateDto
    {
      Firstname = "Updated",
      Lastname = "Name",
      PhoneNumber = "3001111111",
      Role = Domain.Entities.Rol.Administrator,
    };

    var response = await client.PutAsJsonAsync($"{TestConstants.Routes.Employees}/{token.Id}", dto);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var body = await response.Content.ReadFromJsonAsync<EmployeeOverviewDto>();
    Assert.Equal("Updated", body!.FirstName);
  }

  // --- DELETE /api/employees/{id} ---

  [Fact]
  public async Task Delete_EmployeeWithNoReservations_Returns204()
  {
    var superAdminClient = _factory.CreateClientWithCookies();
    await superAdminClient.PostAsJsonAsync(TestConstants.Routes.CreateAdmin, TestDtos.MakeCreateAdminDto(emailSuffix: "del_super"));

    var empDto = new EmployeeCreateDto
    {
      Firstname = "ToDelete",
      Lastname = "Employee",
      PhoneNumber = "3007777777",
      Role = Domain.Entities.Rol.Employee
    };
    var createEmpResponse = await superAdminClient.PostAsJsonAsync(TestConstants.Routes.Employees, empDto);
    var createdEmp = await createEmpResponse.Content.ReadFromJsonAsync<UserTokenDto>();

    var deleteResponse = await superAdminClient.DeleteAsync($"{TestConstants.Routes.Employees}/{createdEmp!.Id}");

    Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
  }
}
