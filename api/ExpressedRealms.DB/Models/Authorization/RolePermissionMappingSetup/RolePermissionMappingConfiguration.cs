using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;

public class RolePermissionMappingConfiguration : IEntityTypeConfiguration<RolePermissionMapping>
{
    public void Configure(EntityTypeBuilder<RolePermissionMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.RoleId).IsRequired();
        builder.Property(e => e.PermissionId).IsRequired();

        builder
            .HasOne(e => e.Permission)
            .WithMany(e => e.RolePermissionMappings)
            .HasForeignKey(e => e.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Role)
            .WithMany(e => e.RolePermissionMappings)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
