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
    }
}
