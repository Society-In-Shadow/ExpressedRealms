using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContactsForCharacterSheet;

internal sealed class GetContactsForCharacterSheetUseCase(
    IContactRepository contactRepository,
    ICharacterRepository characterRepository,
    GetContactsForCharacterSheetModelValidator validator,
    CancellationToken cancellationToken
) : IGetContactsForCharacterSheetUseCase
{
    public async Task<Result<List<ContactListReturnModel>>> ExecuteAsync(GetContactsForCharacterSheetModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var characterInfo = await characterRepository.GetCharacterState(model.CharacterId);

        if (characterInfo.IsInCharacterCreation)
        {
            return Result.Fail("You cannot get contacts while in character creation mode.");
        }

        var contacts = await contactRepository.GetContactsForCharacterSheet(model.CharacterId);

        return Result.Ok(
            contacts
                .Select(x => new ContactListReturnModel()
                {
                    KnowledgeLevel = x.KnowledgeLevel,
                    Name = x.Name,
                    Knowledge = x.Knowledge,
                    Id = x.Id,
                    IsApproved = x.IsApproved,
                    UsesPerWeek = x.UsesPerWeek,
                    KnowledgeDescription = x.KnowledgeDescription,
                    Notes = x.Notes
                })
                .OrderBy(x => x.Name)
                .ToList()
        );
    }
}
