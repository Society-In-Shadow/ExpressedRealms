using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultEventData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO public.event(id, name, start_date, end_date, location, website_name, website_url, additional_notes, con_experience, is_deleted)
            values (1, 'Default Event', '2025-10-24', '2025-10-25', 'Sioux Falls, SD', 'Society in Shadows', 'societyinshadows.org', '', 0, 'true');

            INSERT INTO public.event_schedule_item (event_id, date, start_time, end_time, description, is_deleted)
            VALUES
                (1, '2025-10-24', '14:00', '14:30', 'Setup', 'true'),
                (1, '2025-10-24', '14:30', '15:00', 'GO Huddle', 'true'),
                (1, '2025-10-24', '15:00', '17:00', 'Meet and Greet / SHQ Opens', 'true'),
                (1, '2025-10-24', '15:00', '17:00', 'Game On! (Soft Start)', 'true'),
                (1, '2025-10-24', '19:00', '20:00', 'Soft Break for Food', 'true'),
                (1, '2025-10-24', '23:45', '23:59', 'Game Called / GO Huddle', 'true'),
                (1, '2025-10-25', '10:00', '10:30', 'Break of Dawn', 'true'),
                (1, '2025-10-25', '13:00', '14:00', 'Lunch Break', 'true'),
                (1, '2025-10-25', '17:00', '19:00', 'Dinner Break', 'true'),
                (1, '2025-10-25', '23:45', '23:59', 'Game Called / GO Huddle', 'true'),
                (1, '2025-10-26', '10:00', '10:30', 'Break of Dawn', 'true'),
                (1, '2025-10-26', '12:00', '13:00', 'Lunch Break', 'true'),
                (1, '2025-10-26', '16:00', '16:30', 'GO Huddle', 'true'),
                (1, '2025-10-26', '16:30', '17:00', 'Awards / Wrap-up', 'true');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
