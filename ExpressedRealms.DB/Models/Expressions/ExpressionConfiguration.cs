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

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.ShortDescription).HasMaxLength(125).IsRequired();
        builder.Property(e => e.NavMenuImage).IsRequired();
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.Culture).IsRequired();
        builder.Property(e => e.Alliances).IsRequired();
        builder.Property(e => e.StrainedRelationships).IsRequired();
        builder.Property(e => e.Advantages).IsRequired();
        builder.Property(e => e.Disadvantages).IsRequired();
        builder.Property(e => e.MaterialWeakness);
    }
}
