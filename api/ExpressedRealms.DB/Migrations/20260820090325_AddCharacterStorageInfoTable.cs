using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterStorageInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_storage_infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    opted_in = table.Column<bool>(type: "boolean", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    collector_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    sign_off_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_storage_infos", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_storage_infos_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_storage_infos_players_collector_user_id",
                        column: x => x.collector_user_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_storage_infos_players_sign_off_user_id",
                        column: x => x.sign_off_user_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "character_storage_info_trails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_storage_info_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_storage_info_trails", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_storage_info_trails_character_storage_infos_chara",
                        column: x => x.character_storage_info_id,
                        principalTable: "character_storage_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_storage_info_trails_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_info_trails_actor_user_id",
                table: "character_storage_info_trails",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_info_trails_character_storage_info_id",
                table: "character_storage_info_trails",
                column: "character_storage_info_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_infos_collector_user_id",
                table: "character_storage_infos",
                column: "collector_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_infos_event_id",
                table: "character_storage_infos",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_infos_sign_off_user_id",
                table: "character_storage_infos",
                column: "sign_off_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_storage_info_trails");

            migrationBuilder.DropTable(
                name: "character_storage_infos");
        }
    }
}
