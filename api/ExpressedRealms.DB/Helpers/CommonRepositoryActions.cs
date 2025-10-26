using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB.Helpers;

public static class CommonRepositoryActions
{
    public static async Task CommonSaveChanges<TEntity>(
        this ExpressedRealmsDbContext context,
        TEntity entity,
        CancellationToken cancellationToken
    )
        where TEntity : class
    {
        var entry = context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            // Only update if the object isn't already being tracked
            context.Set<TEntity>().Update(entity);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
