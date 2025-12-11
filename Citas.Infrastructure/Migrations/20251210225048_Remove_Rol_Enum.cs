using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
  /// <inheritdoc />
  public partial class Remove_Rol_Enum : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "type",
          table: "rol"
        );

      migrationBuilder.AlterDatabase()
          .Annotation("Npgsql:Enum:enum__day", "friday,monday,saturday,sunday,thursday,tuesday,wednesday")
          .Annotation("Npgsql:Enum:enum__reservation_state", "cancelled,completed,confirmed,pending")
          .OldAnnotation("Npgsql:Enum:enum__day", "friday,monday,saturday,sunday,thursday,tuesday,wednesday")
          .OldAnnotation("Npgsql:Enum:enum__reservation_state", "cancelled,completed,confirmed,pending")
          .OldAnnotation("Npgsql:Enum:enum__rol", "administrator,employee");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterDatabase()
          .Annotation("Npgsql:Enum:enum__day", "friday,monday,saturday,sunday,thursday,tuesday,wednesday")
          .Annotation("Npgsql:Enum:enum__reservation_state", "cancelled,completed,confirmed,pending")
          .Annotation("Npgsql:Enum:enum__rol", "administrator,employee")
          .OldAnnotation("Npgsql:Enum:enum__day", "friday,monday,saturday,sunday,thursday,tuesday,wednesday")
          .OldAnnotation("Npgsql:Enum:enum__reservation_state", "cancelled,completed,confirmed,pending");
    }
  }
}
