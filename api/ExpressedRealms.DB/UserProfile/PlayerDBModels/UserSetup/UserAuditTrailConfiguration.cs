using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

internal class UserAuditTrailConfiguration : IEntityTypeConfiguration<UserAuditTrail>
{
    public void Configure(EntityTypeBuilder<UserAuditTrail> builder)
    {
        builder.ToTable("User_AuditTrail");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserAuditTrails)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
