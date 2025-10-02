using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreModifierTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into public.stat_modifier(id, name)
                values 
                (12, 'Strike'),
                (13, 'Thrust'),
                (14, 'Throw'),
                (15, 'Shoot'),
                (16, 'Cast'),
                (17, 'Project'),
                (18, 'Dodge'),
                (19, 'Parry'),
                (20, 'Evade Throw'),
                (21, 'Evade Shoot'),
                (22, 'Ward'),
                (23, 'Deflect')
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
