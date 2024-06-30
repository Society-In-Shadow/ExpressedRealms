using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatDescriptionMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatDescriptionMappings",
                columns: table => new
                {
                    StatTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    StatLevelId = table.Column<byte>(type: "smallint", nullable: false),
                    ReasonableExpectation = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatDescriptionMappings", x => new { x.StatTypeId, x.StatLevelId });
                    table.ForeignKey(
                        name: "FK_StatDescriptionMappings_StatLevels_StatLevelId",
                        column: x => x.StatLevelId,
                        principalTable: "StatLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatDescriptionMappings_StateTypes_StatTypeId",
                        column: x => x.StatTypeId,
                        principalTable: "StateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatDescriptionMappings_StatLevelId",
                table: "StatDescriptionMappings",
                column: "StatLevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatDescriptionMappings");
        }
    }
}
