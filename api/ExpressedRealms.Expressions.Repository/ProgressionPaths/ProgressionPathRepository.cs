using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.ProgressionPaths;

public class ProgressionPathRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IProgressionPathRepository
{
    public async Task<int> CreateProgressionPath(ProgressionPath progressionPath)
    {
        context.ProgressionPath.Add(progressionPath);
        await context.SaveChangesAsync(cancellationToken);
        return progressionPath.Id;
    }

    public async Task<ProgressionPath> GetProgressionPathForEditing(int id)
    {
        return await context.ProgressionPath.FirstAsync(x => x.Id == id);
    }

    public async Task<bool> ProgressionPathExists(int id)
    {
        return await context.ProgressionPath.AnyAsync(x => x.Id == id);
    }

    public async Task<List<ProgressionPath>> GetProgressionPathsAndLevelsForExpression(int id)
    {
        return await context
            .ProgressionPath.AsNoTracking()
            .Include(x => x.ProgressionLevels)
            .Where(x => x.ExpressionId == id)
            .ToListAsync();
    }
    
    public async Task<int> CreateProgressionLevel(ProgressionLevel progressionLevel)
    {
        context.ProgressionLevel.Add(progressionLevel);
        await context.SaveChangesAsync(cancellationToken);
        return progressionLevel.Id;
    }
}
