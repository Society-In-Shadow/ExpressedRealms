using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Creatures.CreatureSetup.Audit;

internal class CreatureAuditTrailConfiguration : IEntityTypeConfiguration<CreatureAuditTrail>
{
    public void Configure(EntityTypeBuilder<CreatureAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.CreatureAuditTrails);

        builder.Property(e => e.CreatureId).IsRequired();

        builder
            .HasOne(x => x.Creature)
            .WithMany(x => x.CreatureAuditTrails)
            .HasForeignKey(x => x.CreatureId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
