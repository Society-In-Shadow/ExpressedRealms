using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventSetup;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("event");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
        builder.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();
        builder.Property(e => e.Location).HasColumnName("location").HasMaxLength(1000).IsRequired();
        builder
            .Property(e => e.WebsiteName)
            .HasColumnName("website_name")
            .HasMaxLength(250)
            .IsRequired();
        builder
            .Property(e => e.WebsiteUrl)
            .HasColumnName("website_url")
            .HasMaxLength(500)
            .IsRequired();
        builder
            .Property(e => e.AdditionalNotes)
            .HasColumnName("additional_notes")
            .HasMaxLength(5000)
            .IsRequired();
        builder
            .Property(e => e.TimeZoneId)
            .HasColumnName("time_zone_id")
            .HasMaxLength(250)
            .IsRequired()
            .HasDefaultValue("America/Chicago");
        builder.Property(e => e.ConExperience).HasColumnName("con_experience").IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
