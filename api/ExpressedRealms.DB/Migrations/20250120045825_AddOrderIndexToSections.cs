using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIndexToSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "ExpressionSections",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(@"
                WITH RankedItems AS (
                    SELECT 
                        ""Id"",
                        ROW_NUMBER() OVER (
                            PARTITION BY ""ParentId"" -- Group by ParentId or NULL (root)
                            ORDER BY ""Id"" ASC       -- Sort by Id in ascending order
                        ) AS RowIndex
                    FROM public.""ExpressionSections""
                )
                UPDATE public.""ExpressionSections""
                SET ""OrderIndex"" = RankedItems.RowIndex
                FROM RankedItems
                WHERE public.""ExpressionSections"".""Id"" = RankedItems.""Id"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "ExpressionSections");
        }
    }
}
