namespace Citas.Domain.Entities
{
  /**<summary>
   * Represents a role within the system.
   * </summary>
   */
  public class Rol
  {
    public const int AdministratorId = 1;
    public const string Administrator = "ADMINISTRATOR";

    public const int EmployeeId = 2;
    public const string Employee = "EMPLOYEE";

    public const int SuperAdministratorId = 3;
    public const string SuperAdministrator = "SUPER_ADMINISTRATOR";

    public int Id { get; set; }

    public required string Name { get; set; }
  }
}
