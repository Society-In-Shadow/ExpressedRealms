using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

internal sealed class GetGoCheckinInfoUseCase(
    IContactRepository repository,
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
        var contacts = await repository.GetContactsForCharacterSheet(model.Id);

        return Result.Ok(
            new GetCharacterGoFieldReturnModel()
            {
                Contacts = contacts
                    .Where(x => !x.IsApproved)
                    .Select(x => new ContactCheck() { Id = x.Id, Name = x.Name })
                    .ToList(),
                Knowledges = knowledges
                    .Select(x => new KnowledgeToCheck()
                    {
                        Name = x.Name,
                        IsDoctorateLevel = x.LevelId == 8,
                        IsUnknownKnowledge = x.KnowledgeTypeId == 3,
                        Id = x.Id,
                    })
                    .ToList(),
            }
        );
    }
}
