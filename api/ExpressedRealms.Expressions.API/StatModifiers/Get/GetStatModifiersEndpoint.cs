using ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.StatModifiers.Get;

internal static class GetStatModifiersEndpoint
{
    public static async Task<Results<ValidationProblem, Ok<StatModifiersResponse>>> ExecuteAsync(
        int groupId,
        IGetModifiersUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new GetModifiersModel() { GroupId = groupId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new StatModifiersResponse()
            {
                Modifiers = results
                    .Value.Select(x => new StatModifierReturnModel()
                    {
                        Modifier = x.Modifier,
                        ScaleWithLevel = x.ScaleWithLevel,
                        CreationSpecificBonus = x.CreationSpecificBonus,
                        StatModifierId = x.StatModifierId,
                        Id = x.Id,
                    })
                    .ToList(),
            }
        );
    }
}
