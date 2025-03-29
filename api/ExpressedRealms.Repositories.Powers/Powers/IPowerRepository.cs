using ExpressedRealms.Repositories.Powers.Powers.DTOs;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerCreate;
using FluentResults;

namespace ExpressedRealms.Repositories.Powers.Powers;

public interface IPowerRepository
{
    Task<Result<List<PowerInformation>>> GetPowersAsync(int expressionId);
    Task<Result<int>> CreatePower(CreatePowerModel createPowerModel);
}