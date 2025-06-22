using ExpressedRealms.Powers.Repository.Powers.DTOs;
using ExpressedRealms.Powers.Repository.Powers.DTOs.Options;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
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
}
