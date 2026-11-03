using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Factions.FactionModels;

public class FactionConfiguration : IEntityTypeConfiguration<Faction>
{
    public void Configure(EntityTypeBuilder<Faction> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ExpressionId).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Background).IsRequired().HasMaxLength(10_000);

        builder.Property(e => e.ProcessedPlainText);

        builder
            .Property(p => p.SearchVector)
            .HasComputedColumnSql(
                """
                setweight(to_tsvector('english', coalesce("name", '')), 'A') ||
                setweight(to_tsvector('english', coalesce("processed_plain_text", '')), 'B')
                """,
                stored: true
            );

        builder.HasIndex(p => p.SearchVector).HasMethod("GIN");

        builder
            .HasOne(x => x.CloneSource)
            .WithMany(x => x.Clones)
            .HasForeignKey(x => x.CloneSourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.Factions)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
