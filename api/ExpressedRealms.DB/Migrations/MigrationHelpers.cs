using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpressedRealms.DB.Migrations;

public static class MigrationHelpers
{
    public static void ExecuteEmbeddedSqlScript(this MigrationBuilder migrationBuilder, string scriptName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(scriptName);

        if (stream == null)
            throw new InvalidOperationException($"{scriptName} was not found as embedded resource");

        using var reader = new StreamReader(stream);
        migrationBuilder.Sql(reader.ReadToEnd());
    }
}