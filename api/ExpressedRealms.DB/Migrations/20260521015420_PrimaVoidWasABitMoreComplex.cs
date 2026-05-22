using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class PrimaVoidWasABitMoreComplex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "prima_void",
                table: "characters",
                newName: "void_motes");

            migrationBuilder.AddColumn<byte>(
                name: "prima_fragments",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "prima_motes",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "void_fragments",
                table: "characters",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prima_fragments",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "prima_motes",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "void_fragments",
                table: "characters");

            migrationBuilder.RenameColumn(
                name: "void_motes",
                table: "characters",
                newName: "prima_void");
        }
    }
}
