using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;

public class PowerPrerequisiteConfiguration : IEntityTypeConfiguration<PowerPrerequisite>
{
    public void Configure(EntityTypeBuilder<PowerPrerequisite> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.PowerId).IsRequired();
        builder.Property(e => e.RequiredAmount).IsRequired();

        builder
            .HasOne(e => e.Power)
            .WithOne(e => e.Prerequisite)
            .HasForeignKey<PowerPrerequisite>(x => x.PowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
