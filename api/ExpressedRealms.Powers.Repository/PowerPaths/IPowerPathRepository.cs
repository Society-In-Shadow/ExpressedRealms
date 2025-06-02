using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathLIst;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPaths;

public interface IPowerPathRepository
{
    Task<Result<List<PowerPathInformation>>> GetPowerPathsAsync(int expressionId);
    Task<Result<int>> CreatePowerPathAsync(CreatePowerPathModel createPowerPathModel);
    Task<Result> EditPowerPathAsync(EditPowerPathModel editPowerPathModel);
    Task<Result> DeletePowerPathAsync(int powerPathId);
    Task<Result<EditPowerPathInformation>> GetPowerPathAsync(int powerPathId);
}
