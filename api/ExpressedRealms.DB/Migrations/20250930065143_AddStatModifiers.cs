using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stat_modifier_group",
                table: "progression_level",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stat_modifier_group",
                table: "power",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stat_modifier_group",
                table: "blessing_level",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "stat_modifier",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<int>(type: "integer", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stat_modifier", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stat_modifier_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stat_modifier_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stat_group_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stat_group_id = table.Column<int>(type: "integer", nullable: false),
                    stat_modifier_id = table.Column<int>(type: "integer", nullable: false),
                    modifier = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stat_group_mapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_stat_group_mapping_stat_modifier_group_stat_group_id",
                        column: x => x.stat_group_id,
                        principalTable: "stat_modifier_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stat_group_mapping_stat_modifier_stat_modifier_id",
                        column: x => x.stat_modifier_id,
                        principalTable: "stat_modifier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_progression_level_stat_modifier_group",
                table: "progression_level",
                column: "stat_modifier_group");

            migrationBuilder.CreateIndex(
                name: "IX_power_stat_modifier_group",
                table: "power",
                column: "stat_modifier_group");

            migrationBuilder.CreateIndex(
                name: "IX_blessing_level_stat_modifier_group",
                table: "blessing_level",
                column: "stat_modifier_group");

            migrationBuilder.CreateIndex(
                name: "IX_stat_group_mapping_stat_group_id",
                table: "stat_group_mapping",
                column: "stat_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_stat_group_mapping_stat_modifier_id",
                table: "stat_group_mapping",
                column: "stat_modifier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_stat_modifier_group_stat_modifier_group",
                table: "blessing_level",
                column: "stat_modifier_group",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_stat_modifier_group_stat_modifier_group",
                table: "power",
                column: "stat_modifier_group",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_stat_modifier_group_stat_modifier_group",
                table: "progression_level",
                column: "stat_modifier_group",
                principalTable: "stat_modifier_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_stat_modifier_group_stat_modifier_group",
                table: "blessing_level");

            migrationBuilder.DropForeignKey(
                name: "FK_power_stat_modifier_group_stat_modifier_group",
                table: "power");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_stat_modifier_group_stat_modifier_group",
                table: "progression_level");

            migrationBuilder.DropTable(
                name: "stat_group_mapping");

            migrationBuilder.DropTable(
                name: "stat_modifier_group");

            migrationBuilder.DropTable(
                name: "stat_modifier");

            migrationBuilder.DropIndex(
                name: "IX_progression_level_stat_modifier_group",
                table: "progression_level");

            migrationBuilder.DropIndex(
                name: "IX_power_stat_modifier_group",
                table: "power");

            migrationBuilder.DropIndex(
                name: "IX_blessing_level_stat_modifier_group",
                table: "blessing_level");

            migrationBuilder.DropColumn(
                name: "stat_modifier_group",
                table: "progression_level");

            migrationBuilder.DropColumn(
                name: "stat_modifier_group",
                table: "power");

            migrationBuilder.DropColumn(
                name: "stat_modifier_group",
                table: "blessing_level");
        }
    }
}
