using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOrderToPowers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order_index",
                table: "power",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(@"
				WITH RankedItems AS (
					SELECT 
						""Id"",
						ROW_NUMBER() OVER (
							PARTITION BY power_path_id
							ORDER BY ""Id"" ASC
						) AS RowIndex
					FROM public.power
				)
				UPDATE public.power
				SET order_index = RankedItems.RowIndex
				FROM RankedItems
				WHERE public.power.""Id"" = RankedItems.""Id"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_index",
                table: "power");
        }
    }
}
