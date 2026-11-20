using ExpressedRealms.DB.Models.Creatures.CreatureSetup;
using ExpressedRealms.DB.Models.Creatures.PublishType;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Creature> Creatures { get; set; }
    public DbSet<CreaturePublishType> CreaturePublishTypes { get; set; }
}
