using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillLevelDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillLevelDescriptionMapping",
                columns: table => new
                {
                    SkillLevelId = table.Column<byte>(type: "smallint", nullable: false),
                    SkillTypeId = table.Column<byte>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false, defaultValue: "Everything is awesome!")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillLevelDescriptionMapping", x => new { x.SkillLevelId, x.SkillTypeId });
                    table.ForeignKey(
                        name: "FK_SkillLevelDescriptionMapping_SkillLevel_SkillLevelId",
                        column: x => x.SkillLevelId,
                        principalTable: "SkillLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillLevelDescriptionMapping_SkillType_SkillTypeId",
                        column: x => x.SkillTypeId,
                        principalTable: "SkillType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevelDescriptionMapping_SkillTypeId",
                table: "SkillLevelDescriptionMapping",
                column: "SkillTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillLevelDescriptionMapping");
        }
    }
}
