using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatModifierGroupIdToCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stat_modifier_group_id",
                table: "characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_stat_modifier_group_id",
                table: "characters",
                column: "stat_modifier_group_id");

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_modifier_groups_stat_modifier_group_id",
                table: "characters",
                column: "stat_modifier_group_id",
                principalTable: "stat_modifier_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_modifier_groups_stat_modifier_group_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_stat_modifier_group_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "stat_modifier_group_id",
                table: "characters");
        }
    }
}
