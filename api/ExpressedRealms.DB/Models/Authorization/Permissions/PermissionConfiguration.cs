using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.Permissions;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.PermissionResourceId).IsRequired();
        builder.Property(e => e.Key).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(2000);

        builder
            .HasOne(e => e.Resource)
            .WithMany(e => e.Permissions)
            .HasForeignKey(e => e.PermissionResourceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
