using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeScheduleNullableForEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_employee_schedule_employee_schedule_id",
                table: "employee");

            migrationBuilder.AlterColumn<int>(
                name: "employee_schedule_id",
                table: "employee",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_employee_schedule_employee_schedule_id",
                table: "employee",
                column: "employee_schedule_id",
                principalTable: "employee_schedule",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_employee_schedule_employee_schedule_id",
                table: "employee");

            migrationBuilder.AlterColumn<int>(
                name: "employee_schedule_id",
                table: "employee",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_employee_employee_schedule_employee_schedule_id",
                table: "employee",
                column: "employee_schedule_id",
                principalTable: "employee_schedule",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
