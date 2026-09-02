using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPaidItForwardCharacterStorageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "assigned_xp_types",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "description", "name" },
                values: new object[] { "When a user pays for character storage, they get max of 5 XP on their first purchase", "Initial Character Storage Bonus" });

            migrationBuilder.InsertData(
                table: "assigned_xp_types",
                columns: new[] { "id", "deleted_at", "description", "is_deleted", "name" },
                values: new object[] { 8, null, "This is the XP one gets for every event after the initial purchase, assuming they paid for it from the previous event.", false, "Paid It Forward Character Storage Bonus" });

            migrationBuilder.Sql("""
                                 insert into public.assigned_xp_mappings (player_id, event_id, assigned_xp_type_id, assigned_by_user_id, timestamp, amount, is_deleted)
                                 select player_id, 7, 8, players.user_id, NOW(), 5, false from public.character_storage_infos
                                 join public.players on character_storage_infos.collector_player_id = players.id
                                 where character_storage_infos.opted_in = true
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "assigned_xp_types",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "assigned_xp_types",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "description", "name" },
                values: new object[] { "When a user pays for character storage, they get max of 5 XP", "Bought Character Storage" });
        }
    }
}
