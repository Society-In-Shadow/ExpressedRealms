using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderToCMSTabs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order_index",
                table: "expression",
                type: "integer",
                nullable: false,
                defaultValue: 1);
            
            migrationBuilder.Sql(@"
                WITH RankedItems AS (
                    SELECT 
                        id,
                        ROW_NUMBER() OVER (
                            PARTITION BY expression_type_id -- Group by ParentId or NULL (root)
                            ORDER BY id ASC       -- Sort by Id in ascending order
                        ) AS RowIndex
                    FROM public.expression
                )
                UPDATE public.Expression
                SET order_index = RankedItems.RowIndex
                FROM RankedItems
                WHERE public.expression.id = RankedItems.id;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_index",
                table: "expression");
        }
    }
}
