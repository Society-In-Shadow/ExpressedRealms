using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddNewExpressionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "expression_type",
                columns: new[] { "id", "name", "description" },
                values: new object[,]
                {
                    { 4, "Adversaries", "lorem ipsum",  },
                    { 5, "Factions", "lorem ipsum",  },
                    { 6, "The Society", "lorem ipsum",  },
                    { 7, "Character Setup", "lorem ipsum",  },
                    { 8, "Knowledges", "lorem ipsum",  },
                    { 9, "Advantage / Disadvantage / Mixed Blessings", "lorem ipsum",  },
                    { 10, "Combat", "lorem ipsum",  },
                });
            
            migrationBuilder.InsertData(
                table: "expression",
                columns: new[] { "name", "short_description", "nav_menu_item", "publish_status_id", "expression_type_id" },
                values: new object[,]
                {
                    { "Adversaries", "All of the rules", "pi-prime", 1, 4 },
                    { "Factions", "All the stories", "pi-prime", 1, 5 },
                    { "The Society", "All the stories", "pi-prime", 1, 6 },
                    { "Character Setup", "All the stories", "pi-prime", 1, 7 },
                    { "Knowledges", "All the stories", "pi-prime", 1, 8 },
                    { "Advantage / Disadvantage / Mixed Blessings", "All the stories", "pi-prime", 1, 9 },
                    { "Combat", "All the stories", "pi-prime", 1, 10 },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
