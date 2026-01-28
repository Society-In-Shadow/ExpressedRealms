using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContact;

internal sealed class GetContactUseCase(
    IContactRepository contactRepository,
    ICharacterRepository characterRepository,
    GetContactModelValidator validator,
    CancellationToken cancellationToken
) : IGetContactUseCase
{
    public async Task<Result<ContactReturnModel>> ExecuteAsync(GetContactModel model)
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
            return Result.Fail("You cannot view this contact while in character creation mode.");
        }

        var contact = await contactRepository.FindContactAsync(model.Id);

        return Result.Ok(
            new ContactReturnModel()
            {
                KnowledgeLevelId = contact!.KnowledgeLevelId - 1,
                Name = contact.Name,
                KnowledgeId = contact.KnowledgeId,
                Id = contact.Id,
                IsApproved = contact.IsApproved,
                UsesPerWeek = contact.Frequency,
                Notes = contact.Notes,
            }
        );
    }
}
