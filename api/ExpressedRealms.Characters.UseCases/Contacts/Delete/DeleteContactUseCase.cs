using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.Delete;

internal sealed class DeleteContactUseCase(
    IContactRepository contactRepository,
    ICharacterRepository characterRepository,
    DeleteContactModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteContactUseCase
{
    public async Task<Result> ExecuteAsync(DeleteContactModel model)
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

        contact!.SoftDelete();

        await contactRepository.EditAsync(contact!);

        return Result.Ok();
    }
}
