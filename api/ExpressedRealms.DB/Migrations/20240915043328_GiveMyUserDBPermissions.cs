using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class GiveMyUserDBPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO \"\";");
            migrationBuilder.Sql(
                "ALTER DEFAULT PRIVILEGES FOR ROLE aad_postgresql_86cde IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO \"\";");
            migrationBuilder.Sql(
                "GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA efcore TO \"\";");
            migrationBuilder.Sql(
                "ALTER DEFAULT PRIVILEGES FOR ROLE aad_postgresql_86cde IN SCHEMA efcore GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO \"\";");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
