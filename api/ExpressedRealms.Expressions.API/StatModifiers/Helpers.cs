using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

namespace ExpressedRealms.Expressions.API.StatModifiers;

internal static class Helpers
{
    internal static SourceTableEnum RouteTypeNameToEnum(string name)
    {
        switch (name.ToLowerInvariant())
        {
            case "powers": return SourceTableEnum.Powers;
            case "blessings": return SourceTableEnum.Blessings;
            case "progressionlevels": return SourceTableEnum.ProgressionLevels;
            default: throw new NotImplementedException(nameof(name));
        }
    }
}