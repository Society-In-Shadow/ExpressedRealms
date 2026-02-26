using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinSetup;

public class CheckinConfiguration : IEntityTypeConfiguration<Checkin>
{
    public void Configure(EntityTypeBuilder<Checkin> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.PlayerId).IsRequired();
        builder.Property(e => e.EventId).IsRequired();

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.Checkins)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Player)
            .WithMany(x => x.Checkins)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
