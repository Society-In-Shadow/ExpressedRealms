using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Contacts.Audit;

internal class ContactAuditTrailConfiguration : IEntityTypeConfiguration<ContactAuditTrail>
{
    public void Configure(EntityTypeBuilder<ContactAuditTrail> builder)
    {
        builder.ToTable("contact_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.ContactAuditTrails);

        builder.Property(e => e.ContactId).HasColumnName("contact_id");

        builder
            .HasOne(x => x.Contact)
            .WithMany(x => x.ContactAuditTrails)
            .HasForeignKey(x => x.ContactId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
