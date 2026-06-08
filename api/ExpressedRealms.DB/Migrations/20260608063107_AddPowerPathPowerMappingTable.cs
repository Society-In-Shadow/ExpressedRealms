using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerPathPowerMappingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "power_path_power_mappings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    power_path_id = table.Column<int>(type: "integer", nullable: false),
                    power_id = table.Column<int>(type: "integer", nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_power_path_power_mappings", x => x.id);
                    table.ForeignKey(
                        name: "fk_power_path_power_mappings_power_paths_power_path_id",
                        column: x => x.power_path_id,
                        principalTable: "power_paths",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_power_path_power_mappings_powers_power_id",
                        column: x => x.power_id,
                        principalTable: "powers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_power_path_power_mappings_power_id",
                table: "power_path_power_mappings",
                column: "power_id");

            migrationBuilder.CreateIndex(
                name: "ix_power_path_power_mappings_power_path_id",
                table: "power_path_power_mappings",
                column: "power_path_id");

            migrationBuilder.Sql("""
                                 insert into public.power_path_power_mappings (power_id, power_path_id, order_index)
                                 select id, power_path_id, order_index FROM public.powers
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "power_path_power_mappings");
        }
    }
}
