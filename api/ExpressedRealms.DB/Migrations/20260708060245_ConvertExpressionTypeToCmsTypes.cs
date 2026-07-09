using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class ConvertExpressionTypeToCmsTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expressions_expression_types_expression_type_id",
                table: "expressions");

            migrationBuilder.DropTable(
                name: "expression_types");

            migrationBuilder.RenameColumn(
                name: "expression_type_id",
                table: "expressions",
                newName: "cms_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_expressions_expression_type_id",
                table: "expressions",
                newName: "ix_expressions_cms_type_id");

            migrationBuilder.CreateTable(
                name: "cms_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cms_types", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "cms_types",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Expression Menu Item", "Expression" },
                    { 13, "Sections that should show up in the rule book.", "Rule Book Section" },
                    { 14, "Sections that should show up in the world background.", "World Background Section" }
                });

            migrationBuilder.AddForeignKey(
                name: "fk_expressions_cms_types_cms_type_id",
                table: "expressions",
                column: "cms_type_id",
                principalTable: "cms_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expressions_cms_types_cms_type_id",
                table: "expressions");

            migrationBuilder.DropTable(
                name: "cms_types");

            migrationBuilder.RenameColumn(
                name: "cms_type_id",
                table: "expressions",
                newName: "expression_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_expressions_cms_type_id",
                table: "expressions",
                newName: "ix_expressions_expression_type_id");

            migrationBuilder.CreateTable(
                name: "expression_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expression_types", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "expression_types",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Type for the expression menu", "Expression" },
                    { 2, "Holds all information regarding the system", "System Rules" },
                    { 3, "Holds all information regarding the Treasured Tales", "Treasured Tales" }
                });

            migrationBuilder.AddForeignKey(
                name: "fk_expressions_expression_types_expression_type_id",
                table: "expressions",
                column: "expression_type_id",
                principalTable: "expression_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
