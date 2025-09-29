using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;

namespace ExpressedRealms.Expressions.Repository.ProgressionPaths;

public interface IProgressionPathRepository
{
    Task<int> CreateProgressionPath(ProgressionPath progressionPath);
    Task<ProgressionPath> GetProgressionPathForEditing(int id);
    Task<bool> ProgressionPathExists(int id);
    Task SaveProgressionPathChanges(ProgressionPath progressionPath);
    Task<List<ProgressionPath>> GetProgressionPathsAndLevelsForExpression(int id);
    Task<int> CreateProgressionLevel(ProgressionLevel progressionLevel);
    Task<bool> ProgressionLevelExists(int progressionId, int levelId);
    Task<ProgressionLevel> GetProgressionLevelForEditing(int progressionLevelId);
    Task SaveProgressionLevelChanges(ProgressionLevel progressionLevel);
    Task<bool> ProgressionPathExists(int expressionId, int id);
    Task<string> GetProgressionPathName(int id);
}
