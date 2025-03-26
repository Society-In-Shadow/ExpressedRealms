using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB.Models.Powers.Configuration;

internal static class PowersConfiguration
{
    public static void AddPowerConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PowerLevelConfiguration());
        builder.ApplyConfiguration(new PowerConfiguration());
        builder.ApplyConfiguration(new PowerCategoryConfiguration());
    }
}