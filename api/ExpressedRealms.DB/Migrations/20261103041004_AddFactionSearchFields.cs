using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionSearchFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "processed_plain_text",
                table: "factions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "search_vector",
                table: "factions",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "setweight(to_tsvector('english', coalesce(\"name\", '')), 'A') ||\nsetweight(to_tsvector('english', coalesce(\"processed_plain_text\", '')), 'B')",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "ix_factions_search_vector",
                table: "factions",
                column: "search_vector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_factions_search_vector",
                table: "factions");

            migrationBuilder.DropColumn(
                name: "search_vector",
                table: "factions");

            migrationBuilder.DropColumn(
                name: "processed_plain_text",
                table: "factions");
        }
    }
}
