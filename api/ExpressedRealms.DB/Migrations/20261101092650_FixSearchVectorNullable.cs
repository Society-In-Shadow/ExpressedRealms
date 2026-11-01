using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixSearchVectorNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "search_vector",
                table: "expression_sections",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "setweight(to_tsvector('english', coalesce(\"name\", '')), 'A') ||\nsetweight(to_tsvector('english', coalesce(\"processed_plain_text\", '')), 'B')",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true,
                oldComputedColumnSql: "setweight(to_tsvector('english', coalesce(\"name\", '')), 'A') ||\nsetweight(to_tsvector('english', coalesce(\"processed_plain_text\", '')), 'B')",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "search_vector",
                table: "expression_sections",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "setweight(to_tsvector('english', coalesce(\"name\", '')), 'A') ||\nsetweight(to_tsvector('english', coalesce(\"processed_plain_text\", '')), 'B')",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldComputedColumnSql: "setweight(to_tsvector('english', coalesce(\"name\", '')), 'A') ||\nsetweight(to_tsvector('english', coalesce(\"processed_plain_text\", '')), 'B')",
                oldStored: true);
        }
    }
}
