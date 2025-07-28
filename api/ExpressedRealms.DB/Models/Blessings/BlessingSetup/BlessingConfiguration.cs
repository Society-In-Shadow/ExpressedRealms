using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.BlessingSetup;

public class BlessingConfiguration : IEntityTypeConfiguration<Blessing>
{
    public void Configure(EntityTypeBuilder<Blessing> builder)
    {
        builder.ToTable("blessing");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();

        builder.Property(e => e.Type).HasColumnName("type").HasMaxLength(50).IsRequired();
        builder.Property(e => e.SubCategory).HasColumnName("sub_category").HasMaxLength(75);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
