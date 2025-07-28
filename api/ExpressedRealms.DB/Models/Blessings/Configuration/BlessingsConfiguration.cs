using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB.Models.Blessings.Configuration;

internal static class BlessingsConfiguration
{
    public static void AddBlessingConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}