using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<StatGroupMapping> StatGroupMappings { get; set; }
    public DbSet<StatModifier> StatModifiers { get; set; }
    public DbSet<StatModifierGroup> StatModifierGroups { get; set; }
}
