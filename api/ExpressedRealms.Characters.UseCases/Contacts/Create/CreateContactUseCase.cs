using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.Create;

internal sealed class CreateContactUseCase(
    IXpRepository xpRepository,
    IContactRepository contactRepository,
    ICharacterRepository characterRepository,
    CreateContactModelValidator validator,
    CancellationToken cancellationToken
) : ICreateContactUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateContactModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var xpInfo = await xpRepository.GetAvailableXpForSection(
            model.CharacterId,
            XpSectionTypes.Contacts
        );

        var characterInfo = await characterRepository.GetCharacterState(model.CharacterId);

        if (characterInfo.IsInCharacterCreation)
        {
            return Result.Fail("You cannot add contacts while in character creation mode.");
        }

        byte spentXp = 0;
        spentXp += model.KnowledgeLevel switch
        {
            4 => 6, // Level 4 knowledge (Associates) is 6xp (Base xp needed to purchase contact)
            5 => 10, // Level 5 knowledge (Bachelor) is above + 4 = 10 xp
            6 => 16, // Level 6 knowledge (Masters) is above + 6 = 16 xp
            _ => throw new ArgumentOutOfRangeException(nameof(model.KnowledgeLevel))
        };

        spentXp += model.ContactFrequency switch
        {
            1 => 0, // Once a week is 0 xp
            2 => 4, // Twice a week is 4 xp
            3 => 10, // Three times a week is above + 6 = 10 xp
            _ => throw new ArgumentOutOfRangeException(nameof(model.ContactFrequency))
        };

        if (xpInfo.SpentXp + spentXp > xpInfo.AvailableXp)
            return Result.Fail(
                new NotEnoughXPFailure(xpInfo.AvailableXp - xpInfo.SpentXp, spentXp)
            );

        var mappingId = await contactRepository.CreateAsync(
            new ()
            {
                Name = model.Name.Trim(),
                Frequency = model.ContactFrequency,
                KnowledgeLevelId = model.KnowledgeLevel + 1, // Id is offset by 1
                CharacterId = model.CharacterId,
                KnowledgeId = model.KnowledgeId,
                SpentXp = spentXp,
                Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim(),
            }
        );

        return Result.Ok(mappingId);
    }
}
