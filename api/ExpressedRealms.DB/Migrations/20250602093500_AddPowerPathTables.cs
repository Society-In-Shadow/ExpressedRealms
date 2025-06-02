using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerPathTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_Expressions_ExpressionId",
                table: "power");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "power",
                newName: "power_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_ExpressionId",
                table: "power",
                newName: "IX_power_power_path_id");

            migrationBuilder.CreateTable(
                name: "power_path",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    ExpressionId = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_path", x => x.id);
                    table.ForeignKey(
                        name: "FK_power_path_Expressions_ExpressionId",
                        column: x => x.ExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_power_path_ExpressionId",
                table: "power_path",
                column: "ExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_power_power_path_power_path_id",
                table: "power",
                column: "power_path_id",
                principalTable: "power_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_power_path_power_path_id",
                table: "power");

            migrationBuilder.DropTable(
                name: "power_path");

            migrationBuilder.RenameColumn(
                name: "power_path_id",
                table: "power",
                newName: "ExpressionId");

            migrationBuilder.RenameIndex(
                name: "IX_power_power_path_id",
                table: "power",
                newName: "IX_power_ExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_power_Expressions_ExpressionId",
                table: "power",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
