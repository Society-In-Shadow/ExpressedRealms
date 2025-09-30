using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixStatModifierName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "stat_modifier",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 250);

            migrationBuilder.Sql(@"
                insert into public.stat_modifier(id, name)
                values 
                (1, 'Vitality'),
                (2, 'Health'),
                (3, 'Blood'),
                (4, 'Reaction'),
                (5, 'Psyche'),
                (6, 'RWP'),
                (7, 'Mortis'),
                (8, 'Chi'),
                (9, 'Essence'),
                (10, 'Mana'),
                (11, 'Noumenon')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "name",
                table: "stat_modifier",
                type: "integer",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);
        }
    }
}
