using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class CreateExpressionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Expressions_ExpressionId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_Expressions_ExpressionId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Expressions_ExpressionPublishStatus_PublishStatusId",
                table: "Expressions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_Expressions_ExpressionId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_Expressions_ExpressionId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_Expressions_expression_id",
                table: "power_path");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_Expressions_expression_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expressions",
                table: "Expressions");

            migrationBuilder.RenameTable(
                name: "Expressions",
                newName: "expression");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "expression",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "expression",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "expression",
                newName: "short_description");

            migrationBuilder.RenameColumn(
                name: "PublishStatusId",
                table: "expression",
                newName: "publish_status_id");

            migrationBuilder.RenameColumn(
                name: "NavMenuImage",
                table: "expression",
                newName: "nav_menu_item");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "expression",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "expression",
                newName: "deleted_at");

            migrationBuilder.RenameIndex(
                name: "IX_Expressions_PublishStatusId",
                table: "expression",
                newName: "IX_expression_publish_status_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expression",
                table: "expression",
                column: "id");

            migrationBuilder.CreateTable(
                name: "expression_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expression_type", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "expression_type",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Type for the expression menu", "Expression" },
                    { 2, "Holds all information regarding the system", "System Rules" },
                    { 3, "Holds all information regarding the Treasured Tales", "Treasured Tales" }
                });
            
            migrationBuilder.AddColumn<int>(
                name: "expression_type_id",
                table: "expression",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_expression_expression_type_id",
                table: "expression",
                column: "expression_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_expression_ExpressionId",
                table: "Characters",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_expression_ExpressionPublishStatus_publish_status_id",
                table: "expression",
                column: "publish_status_id",
                principalTable: "ExpressionPublishStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_expression_expression_type_expression_type_id",
                table: "expression",
                column: "expression_type_id",
                principalTable: "expression_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_expression_ExpressionId",
                table: "Expression_AuditTrail",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_expression_ExpressionId",
                table: "ExpressionSection_AuditTrail",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_expression_ExpressionId",
                table: "ExpressionSections",
                column: "ExpressionId",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_expression_expression_id",
                table: "power_path",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_expression_expression_id",
                table: "power_path_audit_trail",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
            
            migrationBuilder.InsertData(
                table: "expression",
                columns: new[] { "name", "short_description", "nav_menu_item", "publish_status_id", "expression_type_id" },
                values: new object[,]
                {
                    { "System Rules", "All of the rules", "pi-prime", 1, 2 },
                    { "Treasured Tales", "All the stories", "pi-prime", 1, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_expression_ExpressionId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_expression_ExpressionPublishStatus_publish_status_id",
                table: "expression");

            migrationBuilder.DropForeignKey(
                name: "FK_expression_expression_type_expression_type_id",
                table: "expression");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_expression_ExpressionId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_expression_ExpressionId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSections_expression_ExpressionId",
                table: "ExpressionSections");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_expression_expression_id",
                table: "power_path");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_expression_expression_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropTable(
                name: "expression_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expression",
                table: "expression");

            migrationBuilder.DropIndex(
                name: "IX_expression_expression_type_id",
                table: "expression");

            migrationBuilder.DropColumn(
                name: "expression_type_id",
                table: "expression");

            migrationBuilder.RenameTable(
                name: "expression",
                newName: "Expressions");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Expressions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Expressions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "short_description",
                table: "Expressions",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "publish_status_id",
                table: "Expressions",
                newName: "PublishStatusId");

            migrationBuilder.RenameColumn(
                name: "nav_menu_item",
                table: "Expressions",
                newName: "NavMenuImage");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Expressions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Expressions",
                newName: "DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_expression_publish_status_id",
                table: "Expressions",
                newName: "IX_Expressions_PublishStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expressions",
                table: "Expressions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Expressions_ExpressionId",
                table: "Characters",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_Expressions_ExpressionId",
                table: "Expression_AuditTrail",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expressions_ExpressionPublishStatus_PublishStatusId",
                table: "Expressions",
                column: "PublishStatusId",
                principalTable: "ExpressionPublishStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_Expressions_ExpressionId",
                table: "ExpressionSection_AuditTrail",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSections_Expressions_ExpressionId",
                table: "ExpressionSections",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_Expressions_expression_id",
                table: "power_path",
                column: "expression_id",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_Expressions_expression_id",
                table: "power_path_audit_trail",
                column: "expression_id",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
