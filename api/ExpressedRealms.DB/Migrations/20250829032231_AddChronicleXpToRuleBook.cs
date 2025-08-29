using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddChronicleXpToRuleBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "expression_type",
                columns: new[] { "id", "name", "description" },
                values: new object[,]
                {
                    { 13, "Rule Book Section", "Sections that should show up in the rule book." },
                    { 14, "World Background Section", "Sections that should show up in the world background." },
                });

            migrationBuilder.Sql(@"update public.expression
                                    set expression_type_id = 13
                                    where id in (5, 13, 14, 15, 16, 17, 18);

                                    update public.expression
                                    set expression_type_id = 14
                                    where id in (6, 10, 11, 12)");
            
            migrationBuilder.InsertData(
                table: "expression",
                columns: new[] { "name", "short_description", "nav_menu_item", "publish_status_id", "expression_type_id" },
                values: new object[,]
                {
                    { "Chronicle XP", "All of the xp info for the Chronicle", "pi-prime", 3, 13 },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
