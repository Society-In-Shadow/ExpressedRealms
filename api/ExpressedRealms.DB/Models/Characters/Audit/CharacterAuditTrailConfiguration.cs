using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.Audit
{
    internal class CharacterAuditTrailConfiguration : IEntityTypeConfiguration<CharacterAuditTrail>
    {
        public void Configure(EntityTypeBuilder<CharacterAuditTrail> builder)
        {
            builder.ConfigureAuditTrailProperties(user => user.CharacterAuditTrails);

            builder.Property(e => e.CharacterId).IsRequired();
        }
    }
}
