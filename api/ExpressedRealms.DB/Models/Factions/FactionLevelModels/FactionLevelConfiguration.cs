using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Factions.FactionLevelModels;

public class FactionLevelConfiguration : IEntityTypeConfiguration<FactionLevel>
{
    public void Configure(EntityTypeBuilder<FactionLevel> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.FactionId).IsRequired();
        builder.Property(e => e.FactionRankId).IsRequired();
        builder.Property(e => e.KnowledgeId);
        builder.Property(e => e.KnowledgeLevelId);
        builder.Property(e => e.Specialization).HasMaxLength(250);
        builder.Property(e => e.CloneSourceId);
        builder.Property(e => e.CloneBatchId);

        builder
            .HasOne(x => x.CloneSource)
            .WithMany(x => x.Clones)
            .HasForeignKey(x => x.CloneSourceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.Faction)
            .WithMany(x => x.FactionLevels)
            .HasForeignKey(x => x.FactionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.FactionRank)
            .WithMany(x => x.FactionLevels)
            .HasForeignKey(x => x.FactionRankId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Knowledge)
            .WithMany(x => x.FactionLevels)
            .HasForeignKey(x => x.KnowledgeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.KnowledgeLevel)
            .WithMany(x => x.FactionLevels)
            .HasForeignKey(x => x.KnowledgeLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Power)
            .WithMany(x => x.FactionLevels)
            .HasForeignKey(x => x.PowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
