namespace Citas.Tests.Helpers;

public static class TestConstants
{
  public const string AdminPassword = "P@ssword123";
  public const string EmployeePhone = "3001234567";
  public const string CompanyPhone = "3009999999";
  public const string UnitCompanyPhone = "1234567890";
  public const string CompanyAddress = "Calle 1";
  public const int NotFoundId = 999999;

  public static class Routes
  {
    public const string CreateAdmin = "/api/employees/create-admin";
    public const string Employees = "/api/employees";
    public const string Login = "/api/auth/login";
    public const string Check = "/api/auth/check";
    public const string Logout = "/api/auth/logout";
    public const string Services = "/api/services";
  }
}
