using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyForEventAndEventScheduleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_event_schedule_items_event_id",
                table: "event_schedule_items",
                column: "event_id");

            migrationBuilder.AddForeignKey(
                name: "fk_event_schedule_items_events_event_id",
                table: "event_schedule_items",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_event_schedule_items_events_event_id",
                table: "event_schedule_items");

            migrationBuilder.DropIndex(
                name: "ix_event_schedule_items_event_id",
                table: "event_schedule_items");
        }
    }
}
