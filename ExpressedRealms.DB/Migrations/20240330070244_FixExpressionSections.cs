using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixExpressionSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_ExpressionSectionTypes_SectionTypeId",
                table: "ExpressionSection");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_ExpressionSection_ParentId",
                table: "ExpressionSection");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_Expressions_ExpressionId",
                table: "ExpressionSection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionSection",
                table: "ExpressionSection");

            migrationBuilder.RenameTable(
                name: "ExpressionSection",
                newName: "ExpressionSections");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_SectionTypeId",
                table: "ExpressionSections",
                newName: "IX_ExpressionSections_SectionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_ParentId",
                table: "ExpressionSections",
                newName: "IX_ExpressionSections_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_ExpressionId",
                table: "ExpressionSections",
                newName: "IX_ExpressionSections_ExpressionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExpressionSections",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionSections",
                table: "ExpressionSections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_ExpressionSectionTypes_SectionTypeId",
                table: "ExpressionSections",
                column: "SectionTypeId",
                principalTable: "ExpressionSectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_ExpressionSections_ParentId",
                table: "ExpressionSections",
                column: "ParentId",
                principalTable: "ExpressionSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_Expressions_ExpressionId",
                table: "ExpressionSections",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_ExpressionSectionTypes_SectionTypeId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_ExpressionSections_ParentId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_Expressions_ExpressionId",
                table: "ExpressionSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionSections",
                table: "ExpressionSections");

            migrationBuilder.RenameTable(
                name: "ExpressionSections",
                newName: "ExpressionSection");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSections_SectionTypeId",
                table: "ExpressionSection",
                newName: "IX_ExpressionSection_SectionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSections_ParentId",
                table: "ExpressionSection",
                newName: "IX_ExpressionSection_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSections_ExpressionId",
                table: "ExpressionSection",
                newName: "IX_ExpressionSection_ExpressionId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ExpressionSection",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionSection",
                table: "ExpressionSection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_ExpressionSectionTypes_SectionTypeId",
                table: "ExpressionSection",
                column: "SectionTypeId",
                principalTable: "ExpressionSectionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_ExpressionSection_ParentId",
                table: "ExpressionSection",
                column: "ParentId",
                principalTable: "ExpressionSection",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_Expressions_ExpressionId",
                table: "ExpressionSection",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
