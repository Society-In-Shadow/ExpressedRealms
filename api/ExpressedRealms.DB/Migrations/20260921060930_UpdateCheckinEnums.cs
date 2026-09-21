using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckinEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "description", "name" },
                values: new object[] { "The CRB has been printed, just needs assembly", "CRB Printed" });

            migrationBuilder.UpdateData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "CRB Read For Pickup (Depreciated)");

            migrationBuilder.UpdateData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "description", "name" },
                values: new object[] { "CRB is fully assembled, including power cards, strips and badge", "CRB has been Assembled" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "description", "name" },
                values: new object[] { "SHQ has received that it needs to print and ready the CRB.", "CRB Creation" });

            migrationBuilder.UpdateData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "CRB Read For Pickup");

            migrationBuilder.UpdateData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "description", "name" },
                values: new object[] { "CRB has printed at least once during this event", "CRB has been printed" });
        }
    }
}
