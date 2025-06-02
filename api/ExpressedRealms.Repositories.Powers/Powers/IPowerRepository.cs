using ExpressedRealms.Repositories.Powers.Powers.DTOs;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.Options;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerCreate;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerEdit;
using FluentResults;

namespace ExpressedRealms.Repositories.Powers.Powers;

public interface IPowerRepository
{
    Task<Result<List<PowerInformation>>> GetPowersAsync(int powerPathId);
    Task<Result<int>> CreatePower(CreatePowerModel createPowerModel);
    Task<Result<int>> EditPower(EditPowerModel editPowerModel);
    Task<Result> DeletePowerAsync(int id);
    Task<Result<PowerOptions>> GetPowerOptionsAsync();
    Task<Result<EditPowerInformation>> GetPowerAsync(int powerId);
}
