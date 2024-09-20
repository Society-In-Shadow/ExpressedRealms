using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlayerPhoneCityStateInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Players",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Players",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Players",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }
    }
}
