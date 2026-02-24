using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventSetup;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();
        builder.Property(e => e.Location).HasMaxLength(1000).IsRequired();
        builder
            .Property(e => e.WebsiteName)
            .HasMaxLength(250)
            .IsRequired();
        builder
            .Property(e => e.WebsiteUrl)
            .HasMaxLength(500)
            .IsRequired();
        builder
            .Property(e => e.AdditionalNotes)
            .HasMaxLength(5000);
        builder
            .Property(e => e.TimeZoneId)
            .HasMaxLength(250)
            .IsRequired()
            .HasDefaultValue("America/Chicago");
        builder.Property(e => e.ConExperience).IsRequired();
        builder
            .Property(e => e.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
