using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddLookupIdToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lookup_id",
                table: "Players",
                type: "char(8)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_lookup_id",
                table: "Players",
                column: "lookup_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_lookup_id",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lookup_id",
                table: "Players");
        }
    }
}
