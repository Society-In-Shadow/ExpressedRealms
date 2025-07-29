using ExpressedRealms.Blessings.Repository.Blessings;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;

internal sealed class GetBlessingsUseCase(IBlessingRepository repository) : IGetBlessingsUseCase
{
    public async Task<Result<GetBlessingsReturnModel>> ExecuteAsync()
    {
        var blessings = await repository.GetAllBlessingsAndBlessingLevels();

        return new GetBlessingsReturnModel()
        {
            Blessings = blessings
                .Select(x => new BlessingReturnModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type,
                    SubCategory = x.SubCategory,
                    Levels = x
                        .BlessingLevels.Select(y => new BlessingLevelReturnModel()
                        {
                            Description = y.Description,
                            Level = y.Level,
                            XpCost = y.XpCost,
                            XpGain = y.XpGain,
                        })
                        .ToList(),
                })
                .ToList(),
        };
    }
}
