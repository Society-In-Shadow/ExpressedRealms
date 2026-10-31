using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCmsTypeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cms_types",
                keyColumn: "id",
                keyValue: 13,
                column: "name",
                value: "Rule Book");

            migrationBuilder.UpdateData(
                table: "cms_types",
                keyColumn: "id",
                keyValue: 14,
                column: "name",
                value: "World Background");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cms_types",
                keyColumn: "id",
                keyValue: 13,
                column: "name",
                value: "Rule Book Section");

            migrationBuilder.UpdateData(
                table: "cms_types",
                keyColumn: "id",
                keyValue: 14,
                column: "name",
                value: "World Background Section");
        }
    }
}
