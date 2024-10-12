using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionConfiguration : IEntityTypeConfiguration<Expression>
{
    public void Configure(EntityTypeBuilder<Expression> builder)
    {
        builder.ToTable("Expressions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.ShortDescription).HasMaxLength(125).IsRequired();
        builder.Property(e => e.NavMenuImage).IsRequired();
        builder.Property(e => e.PublishStatusId).IsRequired().HasDefaultValue(1);

        builder
            .HasOne(x => x.PublishStatus)
            .WithMany(x => x.Expressions)
            .HasForeignKey(x => x.PublishStatusId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
