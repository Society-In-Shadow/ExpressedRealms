using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

internal class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.Id).IsRequired();

        builder.Property(x => x.PlayerNumber).IsRequired(false).HasColumnName("player_number");

        builder
            .Property(x => x.LookupId)
            .HasColumnType("char(8)")
            .HasColumnName("lookup_id")
            .IsRequired();

        builder.HasIndex(x => x.LookupId).IsUnique();
        builder.HasIndex(x => x.PlayerNumber).IsUnique();

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.Player)
            .HasForeignKey<Player>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
