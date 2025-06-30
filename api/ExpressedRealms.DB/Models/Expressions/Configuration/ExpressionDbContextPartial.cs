using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Expression> Expressions { get; set; }
    public DbSet<ExpressionSection> ExpressionSections { get; set; }
    public DbSet<ExpressionSectionType> ExpressionSectionTypes { get; set; }
    public DbSet<ExpressionPublishStatus> ExpressionPublishStatus { get; set; }

    public DbSet<ExpressionSectionAuditTrail> ExpressionSectionAuditTrails { get; set; }
    public DbSet<ExpressionAuditTrail> ExpressionAuditTrails { get; set; }
}
