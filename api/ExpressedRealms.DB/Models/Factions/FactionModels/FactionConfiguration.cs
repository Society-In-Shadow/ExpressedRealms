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
