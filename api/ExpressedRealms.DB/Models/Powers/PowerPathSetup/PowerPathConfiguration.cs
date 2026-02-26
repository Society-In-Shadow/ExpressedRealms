using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPathSetup;

public class PowerPathConfiguration : IEntityTypeConfiguration<PowerPath>
{
    public void Configure(EntityTypeBuilder<PowerPath> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.ExpressionId).IsRequired();
        builder.Property(e => e.OrderIndex).IsRequired();

        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasOne(e => e.Expression)
            .WithMany(e => e.PowerPaths)
            .HasForeignKey(e => e.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
