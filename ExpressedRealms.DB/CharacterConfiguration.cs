using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasData
        (
            new Character
            {
                Id = 1,
                Name = "John Doe"
            },
            new Character
            {
                Id = 2,
                Name = "Jane Doe"
            }
        );
    }
}