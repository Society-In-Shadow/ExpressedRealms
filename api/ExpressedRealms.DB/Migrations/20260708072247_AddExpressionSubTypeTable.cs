using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionSubTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "expression_sub_type_id",
                table: "expressions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "expression_sub_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expression_sub_types", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "expression_sub_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Adepts" },
                    { 2, "Aeternari" },
                    { 3, "Shammas" },
                    { 4, "Sidhe" },
                    { 5, "Sorcerers" },
                    { 6, "Vampyre" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_expressions_expression_sub_type_id",
                table: "expressions",
                column: "expression_sub_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_expressions_expression_sub_types_expression_sub_type_id",
                table: "expressions",
                column: "expression_sub_type_id",
                principalTable: "expression_sub_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("""
                                 update public.expressions set expression_sub_type_id = 1 where id = 3;
                                 update public.expressions set expression_sub_type_id = 2 where id = 2;
                                 update public.expressions set expression_sub_type_id = 3 where id = 4;
                                 update public.expressions set expression_sub_type_id = 4 where id = 7;
                                 update public.expressions set expression_sub_type_id = 5 where id = 8;
                                 update public.expressions set expression_sub_type_id = 6 where id = 9;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expressions_expression_sub_types_expression_sub_type_id",
                table: "expressions");

            migrationBuilder.DropTable(
                name: "expression_sub_types");

            migrationBuilder.DropIndex(
                name: "ix_expressions_expression_sub_type_id",
                table: "expressions");

            migrationBuilder.DropColumn(
                name: "expression_sub_type_id",
                table: "expressions");
        }
    }
}
