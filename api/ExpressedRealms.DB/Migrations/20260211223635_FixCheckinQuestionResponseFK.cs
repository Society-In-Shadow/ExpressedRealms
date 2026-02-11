using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixCheckinQuestionResponseFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkin_question_response_event_question_event_question_id",
                table: "checkin_question_response");

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_question_response_event_question_event_question_id",
                table: "checkin_question_response",
                column: "event_question_id",
                principalTable: "event_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkin_question_response_event_question_event_question_id",
                table: "checkin_question_response");

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_question_response_event_question_event_question_id",
                table: "checkin_question_response",
                column: "event_question_id",
                principalTable: "event_question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
