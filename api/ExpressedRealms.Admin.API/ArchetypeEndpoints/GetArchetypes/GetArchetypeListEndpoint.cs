using ExpressedRealms.Admin.UseCases.Archetypes.GetArchetypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.ArchetypeEndpoints.GetArchetypes;

public static class GetArchetypeListEndpoint
{
    public static async Task<Ok<ArchetypeResponse>> Execute(IGetArchetypesUseCase useCase)
    {
        var result = await useCase.ExecuteAsync();

        return TypedResults.Ok(
            new ArchetypeResponse()
            {
                Archetypes = result
                    .Value.Archetypes.Select(x => new Archetype()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ExpressionName = x.ExpressionName,
                    })
                    .ToList(),
            }
        );
    }
}
