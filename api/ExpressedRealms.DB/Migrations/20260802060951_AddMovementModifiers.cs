using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddMovementModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "stat_modifiers",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 26, "Walking Offensive Proficiency" },
                    { 27, "Walking Defensive Proficiency" },
                    { 28, "Walking Paces" },
                    { 29, "Running Offensive Proficiency" },
                    { 30, "Running Defensive Proficiency" },
                    { 31, "Running Paces" },
                    { 32, "Sprinting Offensive Proficiency" },
                    { 33, "Sprinting Defensive Proficiency" },
                    { 34, "Sprinting Paces" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 34);
        }
    }
}
