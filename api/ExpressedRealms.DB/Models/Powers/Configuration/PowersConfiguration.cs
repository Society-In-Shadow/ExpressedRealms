using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB.Models.Powers.Configuration;

internal static class PowersConfiguration
{
    public static void AddPowerConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PowerActivationTimingTypeConfiguration());
        builder.ApplyConfiguration(new PowerAreaOfEffectTypeConfiguration());
        builder.ApplyConfiguration(new PowerCategoryConfiguration());
        builder.ApplyConfiguration(new PowerCategoryMappingConfiguration());
        builder.ApplyConfiguration(new PowerConfiguration());
        builder.ApplyConfiguration(new PowerDurationConfiguration());
        builder.ApplyConfiguration(new PowerLevelConfiguration());
        builder.ApplyConfiguration(new PowerPrerequisitesConfiguration());
    }
}