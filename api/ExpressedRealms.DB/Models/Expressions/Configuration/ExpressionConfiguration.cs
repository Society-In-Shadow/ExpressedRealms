using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB.Models.Expressions.Configuration;

public static class ExpressionConfiguration
{
    public static void AddExpressionConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ExpressionSetup.ExpressionConfiguration());
        builder.ApplyConfiguration(new ExpressionSectionsConfiguration());
        builder.ApplyConfiguration(new ExpressionSectionTypeConfiguration());
        builder.ApplyConfiguration(new ExpressionPublishStatusConfiguration());
        builder.ApplyConfiguration(new ExpressionTypeConfiguration());

        builder.ApplyConfiguration(new ExpressionAuditTrailConfiguration());
        builder.ApplyConfiguration(new ExpressionSectionAuditTrailConfiguration());
    }
}
