using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;

internal class UserRoleAuditTrailConfiguration : IEntityTypeConfiguration<UserRoleAuditTrail>
{
    public void Configure(EntityTypeBuilder<UserRoleAuditTrail> builder)
    {
        builder.ToTable("UserRoles_AuditTrail");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.RoleId).IsRequired();
        builder.Property(e => e.MappingUserId).IsRequired();

        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserRoleAuditTrails)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.MappingUser)
            .WithMany(x => x.MappedUserRoleAuditTrails)
            .HasForeignKey(x => x.MappingUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.UserRoleAuditTrails)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
