using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Characters.AssignedXp.AssignedXpTypeModels;

public class AssignedXpTypeConfiguration : IEntityTypeConfiguration<AssignedXpType>
{
    public void Configure(EntityTypeBuilder<AssignedXpType> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
