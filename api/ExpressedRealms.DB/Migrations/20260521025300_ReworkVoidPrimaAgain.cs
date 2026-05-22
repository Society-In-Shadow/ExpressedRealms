using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class ReworkVoidPrimaAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prima_motes",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "void_motes",
                table: "characters");

            migrationBuilder.AlterColumn<int>(
                name: "void_fragments",
                table: "characters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "prima_fragments",
                table: "characters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AddColumn<int>(
                name: "motes",
                table: "characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "motes",
                table: "characters");

            migrationBuilder.AlterColumn<byte>(
                name: "void_fragments",
                table: "characters",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<byte>(
                name: "prima_fragments",
                table: "characters",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<byte>(
                name: "prima_motes",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "void_motes",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
