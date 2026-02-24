using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.PermissionResources;

public class PermissionResourceConfiguration : IEntityTypeConfiguration<PermissionResource>
{
    public void Configure(EntityTypeBuilder<PermissionResource> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Description).HasMaxLength(2000);
    }
}
