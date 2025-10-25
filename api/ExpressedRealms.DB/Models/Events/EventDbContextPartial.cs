using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<EventScheduleItem> EventScheduleItems { get; set; }
}
