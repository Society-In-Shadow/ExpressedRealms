using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerPrerequisitesConfiguration : IEntityTypeConfiguration<PowerPrerequisites>
{
    public void Configure(EntityTypeBuilder<PowerPrerequisites> builder)
    {
        builder.ToTable("power_prerequisites");

        builder.HasKey(e => new { e.ParentPowerId, e.ChildPowerId });
        builder.Property(e => e.ParentPowerId).IsRequired();
        builder.Property(e => e.ChildPowerId).IsRequired();

        // Define relationships
        builder
            .HasOne(e => e.ChildPower) // ChildPower is linked by ChildPowerId
            .WithMany() // ChildPower doesn't have navigation to prerequisites
            .HasForeignKey(e => e.ChildPowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.ParentPower) // ParentPower is linked by ParentPowerId
            .WithMany(e => e.PrerequisitePowers) // ParentPower has a collection of prerequisites
            .HasForeignKey(e => e.ParentPowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
