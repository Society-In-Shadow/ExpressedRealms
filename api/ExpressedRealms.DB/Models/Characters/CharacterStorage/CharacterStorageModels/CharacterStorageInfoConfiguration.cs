using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;

public class CharacterStorageInfoConfiguration : IEntityTypeConfiguration<CharacterStorageInfo>
{
    public void Configure(EntityTypeBuilder<CharacterStorageInfo> builder)
    {

        builder
            .HasOne(x => x.Player)
            .WithMany(x => x.CharacterStorageInfos)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.CollectorPlayer)
            .WithMany(x => x.CharacterStorageCollectorUsers)
            .HasForeignKey(x => x.CollectorPlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.CharacterStorageInfo)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.SignOffPlayer)
            .WithMany(x => x.CharacterStorageSignOffUsers)
            .HasForeignKey(x => x.SignOffPlayerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
