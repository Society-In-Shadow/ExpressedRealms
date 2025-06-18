using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

public class ExpressionConfiguration : IEntityTypeConfiguration<Expression>
{
    public void Configure(EntityTypeBuilder<Expression> builder)
    {
        builder.ToTable("expression");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder
            .Property(e => e.ShortDescription)
            .HasColumnName("short_description")
            .HasMaxLength(125)
            .IsRequired();
        builder.Property(e => e.NavMenuImage).HasColumnName("nav_menu_item").IsRequired();
        builder
            .Property(e => e.PublishStatusId)
            .HasColumnName("publish_status_id")
            .IsRequired()
            .HasDefaultValue(1);
        builder
            .Property(e => e.ExpressionTypeId)
            .HasColumnName("expression_type_id")
            .IsRequired()
            .HasDefaultValue(1);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");

        builder
            .HasOne(x => x.PublishStatus)
            .WithMany(x => x.Expressions)
            .HasForeignKey(x => x.PublishStatusId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ExpressionType)
            .WithMany(x => x.Expressions)
            .HasForeignKey(x => x.ExpressionTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
