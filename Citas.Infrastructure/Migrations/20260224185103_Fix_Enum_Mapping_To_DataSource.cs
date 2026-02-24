using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Citas.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Enum_Mapping_To_DataSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Enum types remain in the database; enum mapping moved from EF model
            // annotations to NpgsqlDataSourceBuilder (ADO.NET level only).
            // No DDL changes needed.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No DDL to reverse.
        }
    }
}
