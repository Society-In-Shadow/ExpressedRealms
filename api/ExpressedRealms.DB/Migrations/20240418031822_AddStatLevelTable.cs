using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatLevelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StateType",
                table: "StateType");

            migrationBuilder.RenameTable(
                name: "StateType",
                newName: "StateTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StateTypes",
                table: "StateTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StatLevels",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Bonus = table.Column<int>(type: "integer", nullable: false),
                    XPCost = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatLevels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StateTypes",
                table: "StateTypes");

            migrationBuilder.RenameTable(
                name: "StateTypes",
                newName: "StateType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StateType",
                table: "StateType",
                column: "Id");
        }
    }
}
