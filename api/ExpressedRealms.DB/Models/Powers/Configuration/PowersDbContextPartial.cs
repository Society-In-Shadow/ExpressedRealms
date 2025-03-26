using ExpressedRealms.DB.Models.Powers;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Power> Powers { get; set; }
    public DbSet<PowerActivationTimingType> PowerActivationTimingTypes { get; set; }
    public DbSet<PowerAreaOfEffectType> PowerAreaOfEffectTypes { get; set; }
    public DbSet<PowerCategory> PowerCategories { get; set; }
    public DbSet<PowerCategoryMapping> PowerCategoryMappings { get; set; }
    public DbSet<PowerDuration> PowerDurations { get; set; }
    public DbSet<PowerLevel> PowerLevels { get; set; }
    public DbSet<PowerPrerequisites> PowerPrerequisites { get; set; }
}