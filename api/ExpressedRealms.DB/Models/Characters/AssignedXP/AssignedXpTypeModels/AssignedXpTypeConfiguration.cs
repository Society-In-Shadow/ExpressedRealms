using ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels;

public class AssignedXpTypeConfiguration : IEntityTypeConfiguration<AssignedXpType>
{
    public void Configure(EntityTypeBuilder<AssignedXpType> builder)
    {
        var data = AssignedXpTypeEnum
            .List.Select(x => new AssignedXpType() { Id = x.Value, Name = x.ToString(), Description = x.Description })
            .ToList();
        builder.HasData(data);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
