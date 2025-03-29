using ExpressedRealms.Repositories.Powers.Powers.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Powers.Powers;

public interface IPowerRepository
{
    Task<Result<List<PowerInformation>>> GetPowersAsync(int expressionId);
}