using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;

public class PowerPrerequisiteConfiguration : IEntityTypeConfiguration<PowerPrerequisite>
{
    public void Configure(EntityTypeBuilder<PowerPrerequisite> builder)
    {
        builder.ToTable("power_prerequisite");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.PowerId).HasColumnName("power_id").IsRequired();
        builder.Property(e => e.RequiredAmount).HasColumnName("required_amount").IsRequired();

        builder
            .HasOne(e => e.Power)
            .WithOne(e => e.Prerequisite)
            .HasForeignKey<PowerPrerequisite>(x => x.PowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
