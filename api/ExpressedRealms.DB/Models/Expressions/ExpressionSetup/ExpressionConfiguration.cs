using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

public class ExpressionConfiguration : IEntityTypeConfiguration<Expression>
{
    public void Configure(EntityTypeBuilder<Expression> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.ShortDescription).HasMaxLength(125).IsRequired();
        builder.Property(e => e.NavMenuImage).IsRequired();
        builder.Property(e => e.PublishStatusId).IsRequired().HasDefaultValue(1);
        builder.Property(e => e.CmsTypeId).IsRequired().HasDefaultValue(1);
        builder.Property(e => e.OrderIndex).IsRequired().HasDefaultValue(1);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);

        builder
            .HasOne(x => x.PublishStatus)
            .WithMany(x => x.Expressions)
            .HasForeignKey(x => x.PublishStatusId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.CmsType)
            .WithMany(x => x.Expressions)
            .HasForeignKey(x => x.CmsTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ExpressionSubType)
            .WithMany(x => x.Expressions)
            .HasForeignKey(x => x.ExpressionSubTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
