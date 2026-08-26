using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Characters.XpTables;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

internal sealed class GetGoCheckinInfoUseCase(
    ICharacterRepository characterRepository,
    IXpRepository xpRepository,
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

        var character = await characterRepository.FindCharacterAsync(model.Id);
        if (character is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.Id),
                "Character does not exist."
            );

        var knowledges = await knowledgeRepository.GetGoApprovalKnowledges(model.Id);
        var contacts = await repository.GetContactsForCharacterSheet(model.Id);

        // This technically doesn't matter as this is post character creation, we just need the available xp bit
        var xpCheck = await xpRepository.GetAvailableXpForSection(model.Id, XpSectionTypes.Stats);

        return Result.Ok(
            new GetCharacterGoFieldReturnModel()
            {
                SpentTooMuchXp = xpCheck.AvailableXp - xpCheck.SpentXp < 0,
                StillInCharacterCreation = character.IsInCharacterCreation,
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
