using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldValuesAndUpdateCopyProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_agility_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_constitution_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_dexterity_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_intelligence_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_strength_id",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "fk_characters_stat_levels_willpower_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_agility_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_constitution_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_dexterity_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_intelligence_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_strength_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_willpower_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "agility_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "constitution_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "dexterity_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "intelligence_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "strength_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "willpower_id",
                table: "characters");
            
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(
                "ExpressedRealms.DB.Scripts.CopyCharacterToPlayerProc.sql"
            );

            if (stream == null)
                throw new InvalidOperationException("CopyCharacterToPlayerProc.sql not found as embedded resource");

            using var reader = new StreamReader(stream);
            migrationBuilder.Sql(reader.ReadToEnd());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "agility_id",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "constitution_id",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "dexterity_id",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "intelligence_id",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "strength_id",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "willpower_id",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.CreateIndex(
                name: "ix_characters_agility_id",
                table: "characters",
                column: "agility_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_constitution_id",
                table: "characters",
                column: "constitution_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_dexterity_id",
                table: "characters",
                column: "dexterity_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_intelligence_id",
                table: "characters",
                column: "intelligence_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_strength_id",
                table: "characters",
                column: "strength_id");

            migrationBuilder.CreateIndex(
                name: "ix_characters_willpower_id",
                table: "characters",
                column: "willpower_id");

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_agility_id",
                table: "characters",
                column: "agility_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_constitution_id",
                table: "characters",
                column: "constitution_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_dexterity_id",
                table: "characters",
                column: "dexterity_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_intelligence_id",
                table: "characters",
                column: "intelligence_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_strength_id",
                table: "characters",
                column: "strength_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_characters_stat_levels_willpower_id",
                table: "characters",
                column: "willpower_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
