using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddQuickStartGuideSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "expression_type",
                columns: new[] { "id", "name", "description" },
                values: new object[,]
                {
                    { 11, "Quick Start Guide", "lorem ipsum",  },
                });
            
            migrationBuilder.InsertData(
                table: "expression",
                columns: new[] { "name", "short_description", "nav_menu_item", "publish_status_id", "expression_type_id" },
                values: new object[,]
                {
                    { "Quick Start Guide", "All the stories", "pi-prime", 1, 11 },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
