using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

internal sealed class GetGoCheckinInfoUseCase(
    ICharacterRepository repository,
    ICharacterKnowledgeRepository knowledgeRepository,
    GetGoCheckinInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetGoCheckinInfoUseCase
{
    public async Task<Result<GetCharacterGoFieldReturnModel>> ExecuteAsync(
        GetGoCheckinInfoModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledges = await knowledgeRepository.GetGoApprovalKnowledges(model.Id);

        return Result.Ok(
            new GetCharacterGoFieldReturnModel()
            {
                Knowledges = knowledges.Select(x => new KnowledgeToCheck()
                {
                    Name = x.Name,
                    IsDoctorateLevel = x.LevelId == 8,
                    IsUnknownKnowledge = x.KnowledgeTypeId == 3,
                    Id = x.Id
                }).ToList()
            }
        );
    }
}
