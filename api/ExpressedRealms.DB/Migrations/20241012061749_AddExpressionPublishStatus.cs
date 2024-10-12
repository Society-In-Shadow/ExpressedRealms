using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionPublishStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpressionPublishStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressionPublishStatus", x => x.Id);
                });
            
            migrationBuilder.InsertData("ExpressionPublishStatus", new[] { "Name", "Description" }, new[] { "Published", "This has been released to all users" });
            migrationBuilder.InsertData("ExpressionPublishStatus", new[] { "Name", "Description" }, new[] { "Beta", "This is available to beta users" });
            migrationBuilder.InsertData("ExpressionPublishStatus", new[] { "Name", "Description" }, new[] { "Draft", "This is actively being worked on and only visible to editors" });

            migrationBuilder.AddColumn<int>(
                name: "PublishStatusId",
                table: "Expressions",
                type: "integer",
                nullable: false,
                defaultValue: 1);
            
            migrationBuilder.CreateIndex(
                name: "IX_Expressions_PublishStatusId",
                table: "Expressions",
                column: "PublishStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expressions_ExpressionPublishStatus_PublishStatusId",
                table: "Expressions",
                column: "PublishStatusId",
                principalTable: "ExpressionPublishStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expressions_ExpressionPublishStatus_PublishStatusId",
                table: "Expressions");

            migrationBuilder.DropTable(
                name: "ExpressionPublishStatus");

            migrationBuilder.DropIndex(
                name: "IX_Expressions_PublishStatusId",
                table: "Expressions");

            migrationBuilder.DropColumn(
                name: "PublishStatusId",
                table: "Expressions");
        }
    }
}
