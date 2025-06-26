using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPrerequisiteTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "power_prerequisites");

            migrationBuilder.CreateTable(
                name: "power_prerequisite",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    power_id = table.Column<int>(type: "integer", nullable: false),
                    required_amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_prerequisite", x => x.id);
                    table.ForeignKey(
                        name: "FK_power_prerequisite_power_power_id",
                        column: x => x.power_id,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "power_prerequisite_power",
                columns: table => new
                {
                    prerequisite_id = table.Column<int>(type: "integer", nullable: false),
                    power_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_prerequisite_power", x => new { x.prerequisite_id, x.power_id });
                    table.ForeignKey(
                        name: "FK_power_prerequisite_power_power_power_id",
                        column: x => x.power_id,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                        column: x => x.prerequisite_id,
                        principalTable: "power_prerequisite",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_power_prerequisite_power_id",
                table: "power_prerequisite",
                column: "power_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_power_prerequisite_power_power_id",
                table: "power_prerequisite_power",
                column: "power_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "power_prerequisite_power");

            migrationBuilder.DropTable(
                name: "power_prerequisite");

            migrationBuilder.CreateTable(
                name: "power_prerequisites",
                columns: table => new
                {
                    ParentPowerId = table.Column<int>(type: "integer", nullable: false),
                    ChildPowerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_prerequisites", x => new { x.ParentPowerId, x.ChildPowerId });
                    table.ForeignKey(
                        name: "FK_power_prerequisites_power_ChildPowerId",
                        column: x => x.ChildPowerId,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_prerequisites_power_ParentPowerId",
                        column: x => x.ParentPowerId,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_power_prerequisites_ChildPowerId",
                table: "power_prerequisites",
                column: "ChildPowerId");
        }
    }
}
