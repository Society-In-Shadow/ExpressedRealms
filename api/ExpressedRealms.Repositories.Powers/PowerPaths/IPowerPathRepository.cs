using ExpressedRealms.Repositories.Powers.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Repositories.Powers.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Repositories.Powers.PowerPaths.DTOs.PowerPathLIst;
using FluentResults;

namespace ExpressedRealms.Repositories.Powers.PowerPaths;

public interface IPowerPathRepository
{
    Task<Result<List<PowerPathInformation>>> GetPowerPathsAsync(int expressionId);
    Task<Result<int>> CreatePowerPathAsync(CreatePowerPathModel createPowerPathModel);
    Task<Result> EditPowerPathAsync(EditPowerPathModel editPowerPathModel);
    Task<Result> DeletePowerPathAsync(int powerPathId);
    Task<Result<EditPowerPathInformation>> GetPowerPathAsync(int powerPathId);
}
