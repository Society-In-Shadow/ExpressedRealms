using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.EditFaction;

internal sealed class EditFactionUseCase(
    IFactionRepository factionRepository,
    EditFactionModelValidator validator,
    CancellationToken cancellationToken
) : IEditFactionUseCase
{
    public async Task<Result> ExecuteAsync(EditFactionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var faction = await factionRepository.GetFactionForEditingAsync(model.Id);
        
        if (faction is null)
        {
            return Result.Fail(
                new NotFoundFailure(nameof(EditFactionModel.Id), "This Faction was not found.")
            );
        }
        
        var isDuplicateName = await factionRepository.HasDuplicateName(model.Name, model.Id);
        if (isDuplicateName)
            return ValidationHelper.AddSingleValidationFailure(nameof(model.Name), "This name already exists.");
        
        faction.Name = model.Name;
        faction.Background = model.Background;

        await factionRepository.EditFactionAsync(faction);

        return Result.Ok();
    }
}
