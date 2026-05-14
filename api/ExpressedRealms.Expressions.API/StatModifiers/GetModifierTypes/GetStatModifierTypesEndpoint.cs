using ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.StatModifiers.GetModifierTypes;

internal static class GetStatModifierTypesEndpoint
{
    public static async Task<Results<ValidationProblem, Ok<StatModifiersResponse>>> ExecuteAsync(
        string typeName,
        IGetModifierTypesUseCase getModifierTypesUseCase
    )
    {
        var modifierTypeResults = await getModifierTypesUseCase.ExecuteAsync(new GetModifierTypesModel()
        {
            Source = Helpers.RouteTypeNameToEnum(typeName),
        });

        if (modifierTypeResults.HasValidationError(out var modifierTypeValidationProblem))
            return modifierTypeValidationProblem;
        modifierTypeResults.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new StatModifiersResponse()
            {
                ModifierTypes = modifierTypeResults
                    .Value.ModifierTypes.Select(x => new ListItem() { Id = x.Id, Name = x.Name })
                    .ToList(),
                Expressions = modifierTypeResults
                    .Value.Expressions.Select(x => new ListItem() { Id = x.Key, Name = x.Value })
                    .ToList(),
            }
        );
    }
}
