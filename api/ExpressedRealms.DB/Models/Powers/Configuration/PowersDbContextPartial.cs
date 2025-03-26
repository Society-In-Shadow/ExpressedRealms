using ExpressedRealms.DB.Models.Powers;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<PowerLevel> PowerLevels { get; set; }
    public DbSet<Power> Powers { get; set; }
    public DbSet<PowerCategory> PowerCategories { get; set; }
}