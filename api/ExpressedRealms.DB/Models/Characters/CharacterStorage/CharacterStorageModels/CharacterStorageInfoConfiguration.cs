using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;

public class CharacterStorageInfoConfiguration : IEntityTypeConfiguration<CharacterStorageInfo>
{
    public void Configure(EntityTypeBuilder<CharacterStorageInfo> builder)
    {

        builder
            .HasOne(x => x.CollectorUser)
            .WithMany(x => x.CharacterStorageCollectorUsers)
            .HasForeignKey(x => x.CollectorUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.CharacterStorageInfo)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.SignOffUser)
            .WithMany(x => x.CharacterStorageSignOffUsers)
            .HasForeignKey(x => x.SignOffUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
