using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.Approve;

internal sealed class ApproveContactUseCase(
    IContactRepository contactRepository,
    ICharacterRepository characterRepository,
    ApproveContactModelValidator validator,
    CancellationToken cancellationToken
) : IApproveContactUseCase
{
    public async Task<Result> ExecuteAsync(ApproveContactModel model)
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
            return Result.Fail("You cannot edit contacts while in character creation mode.");
        }

        var contact = await contactRepository.FindContactAsync(model.Id);

        contact!.IsApproved = model.Approved;

        await contactRepository.EditAsync(contact!);

        return Result.Ok();
    }
}
