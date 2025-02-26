using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerAuditConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailChanged",
                table: "User_AuditTrail");

            migrationBuilder.DropColumn(
                name: "LoggedIn",
                table: "User_AuditTrail");

            migrationBuilder.DropColumn(
                name: "LoggedOut",
                table: "User_AuditTrail");

            migrationBuilder.DropColumn(
                name: "PasswordChanged",
                table: "User_AuditTrail");

            migrationBuilder.DropColumn(
                name: "PlayerNumber",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerAuditTrail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ChangedProperties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAuditTrail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerAuditTrail_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerAuditTrail_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAuditTrail_PlayerId",
                table: "PlayerAuditTrail",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAuditTrail_UserId",
                table: "PlayerAuditTrail",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerAuditTrail");

            migrationBuilder.AddColumn<bool>(
                name: "EmailChanged",
                table: "User_AuditTrail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoggedIn",
                table: "User_AuditTrail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LoggedOut",
                table: "User_AuditTrail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordChanged",
                table: "User_AuditTrail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "PlayerNumber",
                table: "Players",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
