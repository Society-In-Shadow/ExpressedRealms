using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class BackfillDefaultQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 INSERT INTO public.event_question (event_id, question, question_type_id, is_deleted)
                                 SELECT
                                     e.id,
                                     q.question,
                                     q.question_type_id,
                                 	false
                                 FROM public.event e
                                 CROSS JOIN (
                                     VALUES
                                         ('What is your badge number / name on your badge?', 2),
                                         ('Are you under the age of 18?', 1),
                                         ('Have you brought in a new player? If so what is there name?', 6)
                                 ) AS q(question, question_type_id)
                                 WHERE NOT EXISTS (
                                     SELECT 1
                                     FROM public.event_question eq
                                     WHERE eq.event_id = e.id
                                       AND eq.question_type_id = q.question_type_id
                                 );
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
