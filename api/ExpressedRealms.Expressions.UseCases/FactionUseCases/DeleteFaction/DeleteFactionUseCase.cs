using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.DeleteFaction;

internal sealed class DeleteFactionUseCase(
    IFactionRepository factionRepository,
    DeleteFactionModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteFactionUseCase
{
    public async Task<Result> ExecuteAsync(DeleteFactionModel model)
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
                new NotFoundFailure(nameof(DeleteFactionModel.Id), "This Faction was not found.")
            );
        }

        faction.SoftDelete();

        await factionRepository.EditFactionAsync(faction);

        return Result.Ok();
    }
}
