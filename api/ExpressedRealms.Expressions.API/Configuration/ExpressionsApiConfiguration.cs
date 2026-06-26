using ExpressedRealms.Expressions.API.ExpressionEndpoints;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints;
using ExpressedRealms.Expressions.API.FactionEndpoints;
using ExpressedRealms.Expressions.API.ProgressionPaths;
using ExpressedRealms.Expressions.API.StatModifiers;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Expressions.API.Configuration;

public static class ExpressionsApiConfiguration
{
    public static void AddExpressionEndPoints(this WebApplication app)
    {
        app.AddExpressionEndpoints();
        app.AddExpressionSubsectionEndpoints();
        app.AddProgressionEndpoints();
        app.AddStatModifiersEndpoints();
        app.AddFactionEndpoints();
    }
}
