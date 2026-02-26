using System.Linq.Expressions;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Interceptors;

public static class AuditTableExtensions
{
    public static void ConfigureAuditTrailProperties<TAuditTrail>(
        this EntityTypeBuilder<TAuditTrail> builder,
        Expression<Func<User, IEnumerable<TAuditTrail>?>>? withManyExpression
    )
        where TAuditTrail : class, IAuditTable
    {
        // Configure the standard audit trail properties
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.ActorUserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        // Configure the relationship to ActorUser
        builder
            .HasOne(x => x.ActorUser)
            .WithMany(withManyExpression)
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
