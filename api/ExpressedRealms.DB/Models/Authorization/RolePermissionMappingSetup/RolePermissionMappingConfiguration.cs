using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;

public class RolePermissionMappingConfiguration : IEntityTypeConfiguration<RolePermissionMapping>
{
    public void Configure(EntityTypeBuilder<RolePermissionMapping> builder)
    {
        builder.ToTable("role_permission_mapping");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(e => e.PermissionId).HasColumnName("permission_id").IsRequired();

        builder
            .HasOne(e => e.Permission)
            .WithMany(e => e.RolePermissionMappings)
            .HasForeignKey(e => e.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Role)
            .WithMany(e => e.RolePermissionMappings)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
