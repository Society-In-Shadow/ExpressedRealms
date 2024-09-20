using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels;

internal class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.PlayerNumber).IsRequired();

        builder.Property(x => x.Id).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.Player)
            .HasForeignKey<Player>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
