using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.Contacts.Dtos;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContacts;

internal sealed class GetContactsUseCase(
    IContactRepository contactRepository,
    ICharacterRepository characterRepository,
    GetContactsModelValidator validator,
    CancellationToken cancellationToken
) : IGetContactsUseCase
{
    public async Task<Result<List<ContactListReturnModel>>> ExecuteAsync(GetContactsModel model)
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

        var contacts = await contactRepository.GetContactsForCharacter(model.CharacterId);

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
                })
                .OrderBy(x => x.Name)
                .ToList()
        );
    }
}
