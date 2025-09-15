using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Characters.xpTables;

public class XpSectionTypeConfiguration : IEntityTypeConfiguration<XpSectionType>
{
    public void Configure(EntityTypeBuilder<XpSectionType> builder)
    {
        builder.ToTable("xp_section_type");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").IsRequired();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(150);
        builder.Property(x => x.SectionCap).HasColumnName("creation_cap").IsRequired();

    }
}
