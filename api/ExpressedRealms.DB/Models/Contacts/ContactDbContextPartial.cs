using ExpressedRealms.DB.Models.Contacts;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Contact> Contacts { get; set; }
}
