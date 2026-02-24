using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup;

public class UserRoleMappingConfiguration : IEntityTypeConfiguration<UserRoleMapping>
{
    public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.RoleId).IsRequired();
        builder.Property(e => e.UserId).HasMaxLength(450).IsRequired();
        builder.Property(e => e.ExpireDate);

        builder
            .HasOne(e => e.User)
            .WithMany(e => e.UserRoleMappings)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Role)
            .WithMany(e => e.UserRoleMappings)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
