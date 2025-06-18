using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionTypeConfiguration : IEntityTypeConfiguration<ExpressionType>
{
    public void Configure(EntityTypeBuilder<ExpressionType> builder)
    {
        builder.ToTable("expression_type");

        builder.HasData(
            new ExpressionType
            {
                Id = 1,
                Name = "Expression",
                Description = "Type for the expression menu",
            },
            new ExpressionType
            {
                Id = 2,
                Name = "System Rules",
                Description = "Holds all information regarding the system",
            },
            new ExpressionType
            {
                Id = 3,
                Name = "Treasured Tales",
                Description = "Holds all information regarding the Treasured Tales",
            }
        );

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder
            .Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(250)
            .IsRequired();
    }
}
