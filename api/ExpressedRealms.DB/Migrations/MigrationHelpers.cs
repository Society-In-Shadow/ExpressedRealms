using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpressedRealms.DB.Migrations;

public static class MigrationHelpers
{
    public static void ExecuteEmbeddedSqlScript(this MigrationBuilder migrationBuilder, EmbeddedFiles files)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(GetScriptName(files));

        if (stream == null)
            throw new InvalidOperationException($"{GetScriptName(files)} was not found as embedded resource");

        using var reader = new StreamReader(stream);
        migrationBuilder.Sql(reader.ReadToEnd());
    }

    private static string GetScriptName(EmbeddedFiles files)
    {
        switch (files)
        {
            case EmbeddedFiles.CharacterXpView: return "ExpressedRealms.DB.Scripts.CharacterXpView.sql";
            case EmbeddedFiles.CopyCharacterToPlayerProc: return "ExpressedRealms.DB.Scripts.CopyCharacterToPlayerProc.sql";
            case EmbeddedFiles.CopyExpression: return "ExpressedRealms.DB.Scripts.CopyExpression.sql";
            case EmbeddedFiles.CopyModifiers: return "ExpressedRealms.DB.Scripts.CopyModifiers.sql";
            default:
                throw new ArgumentOutOfRangeException(nameof(files), files, null);
        }
    }

    public enum EmbeddedFiles
    {
        CharacterXpView = 1,
        CopyCharacterToPlayerProc,
        CopyExpression,
        CopyModifiers
    }
}