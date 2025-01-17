using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditTableWithGenerics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpressionSection_AuditTrail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionId = table.Column<int>(type: "integer", nullable: false),
                    ExpressionId = table.Column<int>(type: "integer", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedProperties = table.Column<string>(type: "text", nullable: false),
                    UserId1 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressionSection_AuditTrail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpressionSection_AuditTrail_ExpressionSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "ExpressionSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpressionSection_AuditTrail_Expressions_ExpressionId",
                        column: x => x.ExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_AuditTrail_ExpressionId",
                table: "ExpressionSection_AuditTrail",
                column: "ExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_AuditTrail_SectionId",
                table: "ExpressionSection_AuditTrail",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_AuditTrail_UserId1",
                table: "ExpressionSection_AuditTrail",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpressionSection_AuditTrail");
        }
    }
}
