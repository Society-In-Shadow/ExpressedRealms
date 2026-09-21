using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class VerifyPendingChangesDiff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "checkin_stages",
                columns: new[] { "id", "description", "name" },
                values: new object[] { 15, "The player has completed all steps, and is fully checked in for all the days of the event.", "Final Stage" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 15);
        }
    }
}
