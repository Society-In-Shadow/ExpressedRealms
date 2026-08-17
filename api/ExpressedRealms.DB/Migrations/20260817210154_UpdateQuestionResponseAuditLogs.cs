using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestionResponseAuditLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkin_question_resp",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkins_checkin_id",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_event_questions_event",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trail_users_actor_user_id",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_question_response_audit_trail",
                table: "checkin_question_response_audit_trail");

            migrationBuilder.RenameTable(
                name: "checkin_question_response_audit_trail",
                newName: "checkin_question_response_audit_trails");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trail_event_question_id",
                table: "checkin_question_response_audit_trails",
                newName: "ix_checkin_question_response_audit_trails_event_question_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trail_checkin_id_event_ques",
                table: "checkin_question_response_audit_trails",
                newName: "ix_checkin_question_response_audit_trails_checkin_id_event_que");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trail_actor_user_id",
                table: "checkin_question_response_audit_trails",
                newName: "ix_checkin_question_response_audit_trails_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_question_response_audit_trails",
                table: "checkin_question_response_audit_trails",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trails_checkin_question_res",
                table: "checkin_question_response_audit_trails",
                columns: new[] { "checkin_id", "event_question_id" },
                principalTable: "checkin_question_responses",
                principalColumns: new[] { "checkin_id", "event_question_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trails_checkins_checkin_id",
                table: "checkin_question_response_audit_trails",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trails_event_questions_even",
                table: "checkin_question_response_audit_trails",
                column: "event_question_id",
                principalTable: "event_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trails_users_actor_user_id",
                table: "checkin_question_response_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trails_checkin_question_res",
                table: "checkin_question_response_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trails_checkins_checkin_id",
                table: "checkin_question_response_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trails_event_questions_even",
                table: "checkin_question_response_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_checkin_question_response_audit_trails_users_actor_user_id",
                table: "checkin_question_response_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_question_response_audit_trails",
                table: "checkin_question_response_audit_trails");

            migrationBuilder.RenameTable(
                name: "checkin_question_response_audit_trails",
                newName: "checkin_question_response_audit_trail");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trails_event_question_id",
                table: "checkin_question_response_audit_trail",
                newName: "ix_checkin_question_response_audit_trail_event_question_id");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trails_checkin_id_event_que",
                table: "checkin_question_response_audit_trail",
                newName: "ix_checkin_question_response_audit_trail_checkin_id_event_ques");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_question_response_audit_trails_actor_user_id",
                table: "checkin_question_response_audit_trail",
                newName: "ix_checkin_question_response_audit_trail_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_question_response_audit_trail",
                table: "checkin_question_response_audit_trail",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkin_question_resp",
                table: "checkin_question_response_audit_trail",
                columns: new[] { "checkin_id", "event_question_id" },
                principalTable: "checkin_question_responses",
                principalColumns: new[] { "checkin_id", "event_question_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_checkins_checkin_id",
                table: "checkin_question_response_audit_trail",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_event_questions_event",
                table: "checkin_question_response_audit_trail",
                column: "event_question_id",
                principalTable: "event_questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_question_response_audit_trail_users_actor_user_id",
                table: "checkin_question_response_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
