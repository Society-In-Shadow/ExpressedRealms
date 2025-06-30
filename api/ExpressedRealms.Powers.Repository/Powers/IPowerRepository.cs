using ExpressedRealms.Powers.Repository.Powers.DTOs.Options;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.Powers;

public interface IPowerRepository
{
    Task<Result<List<PowerInformation>>> GetPowersAsync(int powerPathId);
    Task<Result<int>> CreatePower(CreatePowerModel createPowerModel);
    Task<Result> EditPower(EditPowerModel editPowerModel);
    Task<Result> DeletePowerAsync(int id);
    Task<Result<PowerOptions>> GetPowerOptionsAsync();
    Task<Result<EditPowerInformation>> GetPowerAsync(int powerId);
    Task<Result> UpdatePowerPathSortOrder(EditPowerSortModel dto);
    Task<bool> IsValidPower(int id);
    Task<bool> AreValidPowers(List<int> ids);
    Task<bool> RequirementAlreadyExists(int id);
    Task<bool> IsValidRequirement(int id);
}
