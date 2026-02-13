using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropIndex(
                name: "IX_checkin_event_question_response_audit_trail_checkin_id",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropIndex(
                name: "IX_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_checkin_id_even~",
                table: "checkin_event_question_response_audit_trail",
                columns: new[] { "checkin_id", "event_question_id" });

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail",
                column: "event_question_id");

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                table: "checkin_event_question_response_audit_trail",
                columns: new[] { "checkin_id", "event_question_id" },
                principalTable: "checkin_question_response",
                principalColumns: new[] { "checkin_id", "event_question_id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropIndex(
                name: "IX_checkin_event_question_response_audit_trail_checkin_id_even~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropIndex(
                name: "IX_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_checkin_id",
                table: "checkin_event_question_response_audit_trail",
                column: "checkin_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail",
                columns: new[] { "event_question_id", "checkin_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                table: "checkin_event_question_response_audit_trail",
                columns: new[] { "event_question_id", "checkin_id" },
                principalTable: "checkin_question_response",
                principalColumns: new[] { "checkin_id", "event_question_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
