using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinSecondaryStatsSetup;

public class CheckinSecondaryStatsConfiguration : IEntityTypeConfiguration<CheckinSecondaryStat>
{
    public void Configure(EntityTypeBuilder<CheckinSecondaryStat> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CheckinId).IsRequired();
        builder.Property(e => e.Vitality).IsRequired();
        builder.Property(e => e.Health).IsRequired();
        builder.Property(e => e.Blood).IsRequired();
        builder.Property(e => e.Rwp).IsRequired();
        builder.Property(e => e.Psyche).IsRequired();
        builder.Property(e => e.Mortis).IsRequired();
        builder.Property(e => e.Mana).IsRequired();
        builder.Property(e => e.Chi).IsRequired();
        builder.Property(e => e.Essence).IsRequired();
        builder.Property(e => e.Noumenon).IsRequired();
        
        builder
            .HasOne(x => x.Checkin)
            .WithOne(x => x.CheckinSecondaryStats)
            .HasForeignKey<CheckinSecondaryStat>(x => x.CheckinId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
