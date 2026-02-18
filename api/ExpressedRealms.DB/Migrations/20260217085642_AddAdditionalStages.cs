using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "checkin_stage",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 6, "Player has checked in for the day 2 activities (Usually Saturday)", "Day 2 Checkin" },
                    { 7, "Player has checked in for the day 3 activities (Usually Sunday)", "Day 3 Checkin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "checkin_stage",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "checkin_stage",
                keyColumn: "id",
                keyValue: 7);
        }
    }
}
