using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddReapproveCheckinStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "checkin_stages",
                columns: new[] { "id", "description", "name" },
                values: new object[] { 12, "This is used when a player needs to have their CRB re-printed / approved.  Usually due to retirement or sheet changes.", "Player Needs Reapproval" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 12);
        }
    }
}
