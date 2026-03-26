using ExpressedRealms.Characters.UseCases.Characters.GetArchetypesForExpression;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetArchetypesForExpression;

internal static class GetArchetypesForExpressionEndpoint
{
    internal static async Task<
        Results<Ok<GetArchetypeForExpressionResponse>, NotFound, ValidationProblem>
    > ExecuteAsync(int id, [FromServices] IGetArchetypesForExpressionUseCase repository)
    {
        var response = await repository.ExecuteAsync(new() { Id = id });

        if (response.HasValidationError(out var validation))
            return validation;
        if (response.HasNotFound(out var notFound))
            return notFound;
        response.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetArchetypeForExpressionResponse()
            {
                Archetypes = response
                    .Value.Archetypes.Select(x => new ArchetypeCharacterInfo()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Background = x.Background,
                    })
                    .ToList(),
            }
        );
    }
}
