using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.FinalizeCharacterCreate;

internal sealed class FinalizeCharacterCreateUseCase(
    ICharacterRepository repository,
    IXpRepository xpRepository,
    FinalizeCharacterCreateModelValidator validator,
    CancellationToken cancellationToken
) : IFinalizeCharacterCreateUseCase
{
    public async Task<Result> ExecuteAsync(FinalizeCharacterCreateModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var characterXpViews = await xpRepository.GetCharacterXpMappings(model.CharacterId);

        var xpSpent = characterXpViews.Sum(x => x.TrueTotalSpent);

        var total = characterXpViews.Sum(x => x.TrueSectionCap);

        var overallTotal = total;
        var overallSpent = xpSpent;

        if (overallTotal != overallSpent)
        {
            return Result.Fail(
                $"You still need to spend {overallTotal - overallSpent} xp before finalizing create"
            );
        }

        var user = await repository.GetCharacterForEdit(model.CharacterId);

        user.IsInCharacterCreation = false;

        await xpRepository.FinalizeCreateXp(user.Id);

        await repository.UpdateCharacter(user);

        return Result.Ok();
    }
}
