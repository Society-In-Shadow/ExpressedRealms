using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StateType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateType", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateType");
        }
    }
}
