using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Name_Field_in_Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "employee_schedule",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "employee_schedule");
        }
    }
}
