using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpresssionSectionAndExpressionSectionTypeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Advantages",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "Alliances",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "Culture",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "Disadvantages",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "MaterialWeakness",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "StrainedRelationships",
                table: "Expressions");

            migrationBuilder.CreateTable(
                name: "ExpressionSectionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressionSectionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpressionSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpressionId = table.Column<int>(type: "integer", nullable: false),
                    SectionTypeId = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressionSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpressionSection_ExpressionSectionTypes_SectionTypeId",
                        column: x => x.SectionTypeId,
                        principalTable: "ExpressionSectionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpressionSection_ExpressionSection_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ExpressionSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpressionSection_Expressions_ExpressionId",
                        column: x => x.ExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_ExpressionId",
                table: "ExpressionSection",
                column: "ExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_ParentId",
                table: "ExpressionSection",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_SectionTypeId",
                table: "ExpressionSection",
                column: "SectionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpressionSection");

            migrationBuilder.DropTable(
                name: "ExpressionSectionTypes");

            migrationBuilder.AddColumn<string>(
                name: "Advantages",
                table: "Expressions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Alliances",
                table: "Expressions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Culture",
                table: "Expressions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Expressions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Disadvantages",
                table: "Expressions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialWeakness",
                table: "Expressions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StrainedRelationships",
                table: "Expressions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
