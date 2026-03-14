using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAgeGroupsWithQuestionInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 UPDATE public.players
                                 SET age_group_id = 3
                                 FROM public.checkins
                                 JOIN public.checkin_question_responses 
                                     ON checkins.id = checkin_question_responses.checkin_id
                                 WHERE checkins.player_id = players.id
                                   AND checkin_question_responses.event_question_id = 17
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
