using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:enum__day", "monday,tuesday,wednesday,thursday,friday,saturday,sunday")
                .Annotation("Npgsql:Enum:enum__reservation_state", "cancelled,completed,pending,confirmed")
                .Annotation("Npgsql:Enum:enum__rol", "employee,administrator");

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    last_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<int>(type: "enum__rol", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee_schedule",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_schedule", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_schedule_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    suggested_price = table.Column<float>(type: "numeric(10,2)", nullable: false),
                    is_unavailable = table.Column<bool>(type: "boolean", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    last_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    has_account = table.Column<bool>(type: "boolean", nullable: false),
                    email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    password = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    rol_id = table.Column<int>(type: "integer", nullable: false),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    employee_schedule_id = table.Column<int>(type: "integer", nullable: false),
                    position_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_company_company_id",
                        column: x => x.company_id,
                        principalTable: "company",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_employee_schedule_employee_schedule_id",
                        column: x => x.employee_schedule_id,
                        principalTable: "employee_schedule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_position_position_id",
                        column: x => x.position_id,
                        principalTable: "position",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_employee_rol_rol_id",
                        column: x => x.rol_id,
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_schedule_range",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    day = table.Column<int>(type: "enum__day", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    employee_schedule_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_schedule_range", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_schedule_range_employee_schedule_employee_schedule~",
                        column: x => x.employee_schedule_id,
                        principalTable: "employee_schedule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_schedule_exception",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    range_time = table.Column<NpgsqlRange<DateTime>>(type: "tstzrange", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_schedule_exception", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_schedule_exception_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_service",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rating = table.Column<float>(type: "real", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    service_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_service", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_service_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_service_service_service_id",
                        column: x => x.service_id,
                        principalTable: "service",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    range_time = table.Column<NpgsqlRange<DateTime>>(type: "tstzrange", nullable: false),
                    price = table.Column<double>(type: "numeric(10,2)", nullable: false),
                    client_comment = table.Column<string>(type: "text", nullable: true),
                    rating_from_client = table.Column<float>(type: "real", nullable: true),
                    employee_comment = table.Column<string>(type: "text", nullable: true),
                    rating_from_employee = table.Column<float>(type: "real", nullable: true),
                    state = table.Column<int>(type: "enum__reservation_state", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_client_client_id",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservation_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "fk__employee_company",
                table: "employee",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_employee_schedule",
                table: "employee",
                column: "employee_schedule_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_position",
                table: "employee",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_rol",
                table: "employee",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_schedule_company",
                table: "employee_schedule",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_schedule_exception_employee",
                table: "employee_schedule_exception",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_schedule_range_employee_schedule",
                table: "employee_schedule_range",
                column: "employee_schedule_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_service_employee",
                table: "employee_service",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "fk__employee_service_service",
                table: "employee_service",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "fk__reservation_client",
                table: "reservation",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "fk__reservation_employee",
                table: "reservation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "fk__service_company",
                table: "service",
                column: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_schedule_exception");

            migrationBuilder.DropTable(
                name: "employee_schedule_range");

            migrationBuilder.DropTable(
                name: "employee_service");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "employee_schedule");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "company");
        }
    }
}
