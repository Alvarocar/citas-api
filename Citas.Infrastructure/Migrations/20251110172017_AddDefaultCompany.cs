using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
  /// <inheritdoc />
  public partial class AddDefaultCompany : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData(
        table: "company",
        columns: new[] { "id", "name", "address", "phone_number", "email" },
        values: new object[] { 1, "Default Company", "123 Main St, City, Country", "+1-555-1234", "company@email.com" }
       );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DeleteData(
        table: "company",
        keyColumn: "id",
        keyValue: 1);
    }
  }
}
