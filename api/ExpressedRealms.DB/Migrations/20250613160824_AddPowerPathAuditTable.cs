using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerPathAuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PowerPathAuditTrail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpressionId = table.Column<int>(type: "integer", nullable: false),
                    PowerPathId = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActorUserId = table.Column<string>(type: "text", nullable: false),
                    ChangedProperties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerPathAuditTrail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerPathAuditTrail_AspNetUsers_ActorUserId",
                        column: x => x.ActorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerPathAuditTrail_Expressions_ExpressionId",
                        column: x => x.ExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PowerPathAuditTrail_power_path_PowerPathId",
                        column: x => x.PowerPathId,
                        principalTable: "power_path",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerPathAuditTrail_ActorUserId",
                table: "PowerPathAuditTrail",
                column: "ActorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPathAuditTrail_ExpressionId",
                table: "PowerPathAuditTrail",
                column: "ExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_PowerPathAuditTrail_PowerPathId",
                table: "PowerPathAuditTrail",
                column: "PowerPathId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerPathAuditTrail");
        }
    }
}
