using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFaction;

internal sealed class GetFactionUseCase(
    IFactionRepository factionRepository,
    GetFactionModelValidator validator,
    CancellationToken cancellationToken
) : IGetFactionUseCase
{
    public async Task<Result<GetFactionReturnModel>> ExecuteAsync(GetFactionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var faction = await factionRepository.GetFactionAsync(model.Id);

        if (faction is null)
        {
            return Result.Fail(
                new NotFoundFailure(nameof(GetFactionModel.Id), "This Faction was not found.")
            );
        }

        return Result.Ok(
            new GetFactionReturnModel()
            {
                Id = faction.Id,
                Name = faction.Name,
                Background = faction.Background
            }
        );
    }
}
