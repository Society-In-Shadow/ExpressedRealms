using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;

internal class ExpressionSectionAuditTrailConfiguration
    : IEntityTypeConfiguration<ExpressionSectionAuditTrail>
{
    public void Configure(EntityTypeBuilder<ExpressionSectionAuditTrail> builder)
    {
        builder.ToTable("ExpressionSection_AuditTrail");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.ExpressionId).IsRequired();
        builder.Property(e => e.SectionId).IsRequired();

        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.SectionAudits)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ExpressionSection)
            .WithMany(x => x.SectionAudits)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.ExpressionSectionAuditTrails)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
