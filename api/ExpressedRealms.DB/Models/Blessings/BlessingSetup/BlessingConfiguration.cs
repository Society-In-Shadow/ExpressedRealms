using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.BlessingSetup;

public class BlessingConfiguration : IEntityTypeConfiguration<Blessing>
{
    public void Configure(EntityTypeBuilder<Blessing> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();

        builder.Property(e => e.Type).HasMaxLength(50).IsRequired();
        builder
            .Property(e => e.SubCategory)
            .IsRequired()
            .HasMaxLength(75);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
