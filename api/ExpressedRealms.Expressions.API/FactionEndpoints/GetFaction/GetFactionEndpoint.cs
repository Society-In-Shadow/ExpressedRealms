using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.FactionEndpoints.GetFaction;

public static class GetFactionEndpoint
{
    public static async Task<Ok<GetFactionResponse>> ExecuteAsync(
        int id,
        IGetFactionUseCase createFactionUseCase
    )
    {
        var results = await createFactionUseCase.ExecuteAsync(new GetFactionModel() { Id = id });

        return TypedResults.Ok(
            new GetFactionResponse()
            {
                Id = results.Value.Id,
                Name = results.Value.Name,
                Background = results.Value.Background
            }
        );
    }
}
