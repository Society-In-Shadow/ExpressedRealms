using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "AgilityId",
                table: "Characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "ConstitutionId",
                table: "Characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "DexterityId",
                table: "Characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "IntelligenceId",
                table: "Characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "StrengthId",
                table: "Characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.AddColumn<byte>(
                name: "WillpowerId",
                table: "Characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AgilityId",
                table: "Characters",
                column: "AgilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ConstitutionId",
                table: "Characters",
                column: "ConstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_DexterityId",
                table: "Characters",
                column: "DexterityId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IntelligenceId",
                table: "Characters",
                column: "IntelligenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_StrengthId",
                table: "Characters",
                column: "StrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_WillpowerId",
                table: "Characters",
                column: "WillpowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_AgilityId",
                table: "Characters",
                column: "AgilityId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_ConstitutionId",
                table: "Characters",
                column: "ConstitutionId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_DexterityId",
                table: "Characters",
                column: "DexterityId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_IntelligenceId",
                table: "Characters",
                column: "IntelligenceId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_StrengthId",
                table: "Characters",
                column: "StrengthId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_StatLevels_WillpowerId",
                table: "Characters",
                column: "WillpowerId",
                principalTable: "StatLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_AgilityId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_ConstitutionId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_DexterityId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_IntelligenceId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_StrengthId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_StatLevels_WillpowerId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_AgilityId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_ConstitutionId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_DexterityId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_IntelligenceId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_StrengthId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_WillpowerId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "AgilityId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ConstitutionId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DexterityId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IntelligenceId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "StrengthId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WillpowerId",
                table: "Characters");
        }
    }
}
