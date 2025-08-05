using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public class GetKnowledgeLevelsUseCase(
    IKnowledgeLevelRepository levelRepository,
    ICharacterKnowledgeRepository mappingRepository,
    GetKnowledgeLevelsModelValidator validator,
    CancellationToken cancellationToken
    )
    : IGetKnowledgeLevelsUseCase
{
    public async Task<Result<GetKnowledgeLevelsReturnModel>> ExecuteAsync(GetKnowledgeLevelsModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        var knowledgeLevels = await levelRepository.GetKnowledgeLevels();
        var currentExperience = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(model.CharacterId);

        return Result.Ok(
            new GetKnowledgeLevelsReturnModel()
            {
                AvailableExperience = 7 - currentExperience,
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
                        TotalUnknownXpCost = x.TotalUnknownXpCost,
                    })
                    .ToList(),
            }
        );
    }
}
