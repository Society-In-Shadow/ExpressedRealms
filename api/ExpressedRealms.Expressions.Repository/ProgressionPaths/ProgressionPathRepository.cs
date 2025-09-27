using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;

namespace ExpressedRealms.Expressions.Repository.ProgressionPaths;

public class ProgressionPathRepository (
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken) : IProgressionPathRepository
{
    public async Task<int> CreateProgressionPath(ProgressionPath progressionPath)
    {
        context.ProgressionPath.Add(progressionPath);
        await context.SaveChangesAsync(cancellationToken);
        return progressionPath.Id;
    }
}