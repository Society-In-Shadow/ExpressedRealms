using ExpressedRealms.Expressions.API.ExpressionEndpoints;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Expressions.API.Configuration;

public static class ExpressionsAPIConfiguration
{
    public static void AddExpressionEndPoints(this WebApplication app)
    {
        app.AddExpressionEndpoints();
        app.AddExpressionSubsectionEndpoints();
    }
}
