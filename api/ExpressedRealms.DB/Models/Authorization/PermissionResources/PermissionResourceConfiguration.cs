using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.PermissionResources;

public class PermissionResourceConfiguration : IEntityTypeConfiguration<PermissionResource>
{
    public void Configure(EntityTypeBuilder<PermissionResource> builder)
    {
        builder.ToTable("permission_resource");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(500);
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(2000);
    }
}
