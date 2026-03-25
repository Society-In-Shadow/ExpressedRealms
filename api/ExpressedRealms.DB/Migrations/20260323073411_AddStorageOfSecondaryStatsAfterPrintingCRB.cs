using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageOfSecondaryStatsAfterPrintingCRB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "checkin_secondary_stat",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkin_id = table.Column<int>(type: "integer", nullable: false),
                    vitality = table.Column<int>(type: "integer", nullable: false),
                    health = table.Column<int>(type: "integer", nullable: false),
                    blood = table.Column<int>(type: "integer", nullable: false),
                    rwp = table.Column<int>(type: "integer", nullable: false),
                    psyche = table.Column<int>(type: "integer", nullable: false),
                    mortis = table.Column<int>(type: "integer", nullable: false),
                    mana = table.Column<int>(type: "integer", nullable: false),
                    chi = table.Column<int>(type: "integer", nullable: false),
                    essence = table.Column<int>(type: "integer", nullable: false),
                    noumenon = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_checkin_secondary_stat", x => x.id);
                    table.ForeignKey(
                        name: "fk_checkin_secondary_stat_checkins_checkin_id",
                        column: x => x.checkin_id,
                        principalTable: "checkins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_checkin_secondary_stat_checkin_id",
                table: "checkin_secondary_stat",
                column: "checkin_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checkin_secondary_stat");
        }
    }
}
