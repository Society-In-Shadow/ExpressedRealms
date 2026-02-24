using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateXpViewWithNewTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(
                "ExpressedRealms.DB.Scripts.CharacterXpView.sql"
            );

            if (stream == null)
                throw new InvalidOperationException("CharacterXpView.sql not found as embedded resource");

            using var reader = new StreamReader(stream);
            migrationBuilder.Sql(reader.ReadToEnd());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Updating the view is a one way trip
        }
    }
}
