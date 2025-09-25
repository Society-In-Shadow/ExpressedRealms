using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressionPathTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "progression_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    progression_path_id = table.Column<int>(type: "integer", nullable: false),
                    xl_level = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progression_level", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "progression_path",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    expression_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progression_path", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "progression_level_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    progression_level_id = table.Column<int>(type: "integer", nullable: false),
                    progression_path_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progression_level_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_progression_level_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_progression_level_audit_trail_progression_level_progression~",
                        column: x => x.progression_level_id,
                        principalTable: "progression_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_progression_level_audit_trail_progression_path_progression_~",
                        column: x => x.progression_path_id,
                        principalTable: "progression_path",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "progression_path_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    expression_id = table.Column<int>(type: "integer", nullable: false),
                    progression_path_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progression_path_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_progression_path_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_progression_path_audit_trail_expression_expression_id",
                        column: x => x.expression_id,
                        principalTable: "expression",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_progression_path_audit_trail_progression_path_progression_p~",
                        column: x => x.progression_path_id,
                        principalTable: "progression_path",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_progression_level_audit_trail_actor_user_id",
                table: "progression_level_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_progression_level_audit_trail_progression_level_id",
                table: "progression_level_audit_trail",
                column: "progression_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_progression_level_audit_trail_progression_path_id",
                table: "progression_level_audit_trail",
                column: "progression_path_id");

            migrationBuilder.CreateIndex(
                name: "IX_progression_path_audit_trail_actor_user_id",
                table: "progression_path_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_progression_path_audit_trail_expression_id",
                table: "progression_path_audit_trail",
                column: "expression_id");

            migrationBuilder.CreateIndex(
                name: "IX_progression_path_audit_trail_progression_path_id",
                table: "progression_path_audit_trail",
                column: "progression_path_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "progression_level_audit_trail");

            migrationBuilder.DropTable(
                name: "progression_path_audit_trail");

            migrationBuilder.DropTable(
                name: "progression_level");

            migrationBuilder.DropTable(
                name: "progression_path");
        }
    }
}
