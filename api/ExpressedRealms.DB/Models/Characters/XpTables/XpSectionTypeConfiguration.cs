using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.XpTables;

public class XpSectionTypeConfiguration : IEntityTypeConfiguration<XpSectionType>
{
    public void Configure(EntityTypeBuilder<XpSectionType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.SectionCap).IsRequired();
    }
}
