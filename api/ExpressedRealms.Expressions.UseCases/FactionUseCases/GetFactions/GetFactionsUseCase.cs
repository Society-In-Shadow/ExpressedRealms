using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;

internal sealed class GetFactionsUseCase(
    IFactionRepository factionRepository,
    GetFactionsModelValidator validator,
    CancellationToken cancellationToken
) : IGetFactionsUseCase
{
    public async Task<Result<FactionsReturnModel>> ExecuteAsync(GetFactionsModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var factions = await factionRepository.GetFactions(model.ExpressionId);

        return Result.Ok(
            new FactionsReturnModel()
            {
                Factions = factions
                    .Select(x => new FactionModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Background = x.Background,
                        FactionLevels = x
                            .Levels.Select(y => new FactionLevelModel()
                            {
                                Id = y.Id,
                                RankName = y.RankName,
                                KnowledgeId = y.KnowledgeId,
                                Knowledge = y.Knowledge,
                                KnowledgeLevel = y.KnowledgeLevel,
                                Specialization = y.Specialization,
                                KnowledgeLevelId = y.KnowledgeLevelId,
                                PowerId = y.PowerId
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}
