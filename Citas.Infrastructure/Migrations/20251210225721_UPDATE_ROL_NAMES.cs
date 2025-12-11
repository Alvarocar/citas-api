using Citas.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
  /// <inheritdoc />
  public partial class UPDATE_ROL_NAMES : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DeleteData(
          table: "rol",
          keyColumn: "id",
          keyValue: 1
        );
      migrationBuilder.DeleteData(
          table: "rol",
          keyColumn: "id",
          keyValue: 2
        );

      migrationBuilder.InsertData(
          table: "rol",
          columns: new[] { "id", "name" },
          values: new object[] { Rol.AdministratorId, Rol.Administrator }
       );

      migrationBuilder.InsertData(
          table: "rol",
          columns: new[] { "id", "name" },
          values: new object[] { Rol.EmployeeId, Rol.Employee }
       );

      migrationBuilder.InsertData(
          table: "rol",
          columns: new[] { "id", "name" },
          values: new object[] { Rol.SuperAdministratorId, Rol.SuperAdministrator }
       );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData(
          table: "rol",
          columns: new[] { "id", "name" },
          values: new object[] { 1, "Administrator" }
       );

      migrationBuilder.InsertData(
          table: "rol",
          columns: new[] { "id", "name" },
          values: new object[] { 2, "Employee" }
       );

      migrationBuilder.DeleteData(
          table: "rol",
          keyColumn: "id",
          keyValue: Rol.AdministratorId
        );

      migrationBuilder.DeleteData(
          table: "rol",
          keyColumn: "id",
          keyValue: Rol.EmployeeId
        );

      migrationBuilder.DeleteData(
          table: "rol",
          keyColumn: "id",
          keyValue: Rol.SuperAdministratorId
        );
    }
  }
}
