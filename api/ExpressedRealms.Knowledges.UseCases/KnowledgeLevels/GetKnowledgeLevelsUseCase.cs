using ExpressedRealms.Knowledges.Repository;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public class GetKnowledgeLevelsUseCase(IKnowledgeLevelRepository levelRepository)
    : IGetKnowledgeLevelsUseCase
{
    public async Task<Result<GetKnowledgeLevelsModel>> ExecuteAsync()
    {
        var knowledgeLevels = await levelRepository.GetKnowledgeLevels();

        return Result.Ok(
            new GetKnowledgeLevelsModel()
            {
                KnowledgeLevels = knowledgeLevels
                    .Select(x => new KnowledgeLevelModel()
                    {
                        
                        Id = x.Id,
                        Name = x.Name,
                        Level = x.Level,
                        SpecializationCount = x.SpecializationCount,
                        StoneModifier = x.StoneModifier,
                        GeneralXpCost = x.GeneralXpCost,
                        TotalGeneralXpCost = x.TotalGeneralXpCost,
                        UnknownXpCost = x.UnknownXpCost,
                        TotalUnknownXpCost = x.TotalUnknownXpCost
                    })
                    .ToList(),
            }
        );
    }
}
