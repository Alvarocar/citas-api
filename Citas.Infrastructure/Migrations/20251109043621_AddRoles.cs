using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
  /// <inheritdoc />
  public partial class AddRoles : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData(
        table: "rol",
        columns: new[] { "id", "name", "type" },
        values: new object[,]
        {
          { 1, "Administrator", "'administrator'" },
          { 2, "Employee", "'employee'" },
        });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DeleteData(
        table: "rol",
        keyColumn: "id",
        keyValue: 1);

      migrationBuilder.DeleteData(
          table: "rol",
          keyColumn: "id",
          keyValue: 2);
    }
  }
}
