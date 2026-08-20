using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedXpTypeEnumValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "assigned_xp_types",
                columns: new[] { "id", "deleted_at", "description", "is_deleted", "name" },
                values: new object[,]
                {
                    { 7, null, "When a user pays for character storage, they get max of 5 XP", false, "Bought Character Storage" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "assigned_xp_types",
                keyColumn: "id",
                keyValue: 7);
        }
    }
}
