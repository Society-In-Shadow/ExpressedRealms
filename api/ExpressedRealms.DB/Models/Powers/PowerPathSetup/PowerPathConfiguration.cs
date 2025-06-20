using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPathSetup;

public class PowerPathConfiguration : IEntityTypeConfiguration<PowerPath>
{
    public void Configure(EntityTypeBuilder<PowerPath> builder)
    {
        builder.ToTable("power_path");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();
        builder.Property(e => e.ExpressionId).HasColumnName("expression_id").IsRequired();
        builder.Property(e => e.OrderIndex).HasColumnName("order_index").IsRequired();

        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasOne(e => e.Expression)
            .WithMany(e => e.PowerPaths)
            .HasForeignKey(e => e.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
