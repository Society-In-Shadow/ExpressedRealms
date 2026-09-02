using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPaidItForwardCharacterStorageTypeRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "assigned_xp_types",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "description", "name" },
                values: new object[] { "XP User gets for opting into character storage at the last con.", "Ongoing Character Storage Bonus" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "assigned_xp_types",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "description", "name" },
                values: new object[] { "This is the XP one gets for every event after the initial purchase, assuming they paid for it from the previous event.", "Paid It Forward Character Storage Bonus" });
        }
    }
}
