using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.Permissions;

public class PermissionConfiguration
    : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permission");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.PermissionResourceId).HasColumnName("permission_resource_id").IsRequired();
        builder.Property(e => e.Key).HasColumnName("key").IsRequired().HasMaxLength(500);
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(500);
        builder.Property(e => e.Description).HasColumnName("description").IsRequired().HasMaxLength(2000);

        builder
            .HasOne(e => e.Resource)
            .WithMany(e => e.Permissions)
            .HasForeignKey(e => e.PermissionResourceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
