using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;

public class CheckinStageMappingConfiguration : IEntityTypeConfiguration<CheckinStageMapping>
{
    public void Configure(EntityTypeBuilder<CheckinStageMapping> builder)
    {
        builder.ToTable("checkin_stage_mapping");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.CheckinId).HasColumnName("checkin_id").IsRequired();
        builder.Property(e => e.CheckinStageId).HasColumnName("checkin_stage_id").IsRequired();
        builder
            .Property(e => e.ApproverUserId)
            .HasColumnName("approver_user_id")
            .IsRequired()
            .HasMaxLength(450);
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();

        builder
            .HasOne(x => x.Checkin)
            .WithMany(x => x.CheckinStageMappings)
            .HasForeignKey(x => x.CheckinId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.CheckinStage)
            .WithMany(x => x.CheckinStageMappings)
            .HasForeignKey(x => x.CheckinStageId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ApproverUser)
            .WithMany(x => x.CheckinStageMappings)
            .HasForeignKey(x => x.ApproverUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
