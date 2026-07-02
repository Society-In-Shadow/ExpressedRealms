using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;

internal sealed class GetFactionsUseCase(
    IFactionRepository factionRepository,
    IPowerRepository powersRepository,
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

        var powerIds = factions
            .SelectMany(x => x.Levels.Where(x => x.PowerId != null).Select(y => y.PowerId!.Value))
            .ToList();

        var powers = await powersRepository.GetPowers(powerIds);

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
                                Power = powers.FirstOrDefault(z => z.Id == y.PowerId),
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}
