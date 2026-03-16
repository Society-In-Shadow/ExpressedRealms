using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalCheckinSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "checkin_stages",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 8, "User has completed the age check approval process, or player has been previously approved as an adult", "Age Check Approval" },
                    { 9, "Event Question have been answered", "Event Questions Check" },
                    { 10, "Player has been assigned XP", "Assign XP Check" },
                    { 11, "CRB has printed at least once during this event", "CRB has been printed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 11);
        }
    }
}
