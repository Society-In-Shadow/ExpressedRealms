using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;

namespace ExpressedRealms.Expressions.Repository.ProgressionPaths;

public interface IProgressionPathRepository
{
    Task<int> CreateProgressionPath(ProgressionPath progressionPath);
}