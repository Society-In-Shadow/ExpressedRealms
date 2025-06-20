using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddSortToPowerPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order_index",
                table: "power_path",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(@"
				WITH RankedItems AS (
					SELECT 
						id,
						ROW_NUMBER() OVER (
							PARTITION BY expression_id
							ORDER BY id ASC
						) AS RowIndex
					FROM public.power_path
				)
				UPDATE public.power_path
				SET order_index = RankedItems.RowIndex
				FROM RankedItems
				WHERE public.power_path.id = RankedItems.id;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_index",
                table: "power_path");
        }
    }
}
