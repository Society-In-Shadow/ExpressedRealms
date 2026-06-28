using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.FactionEndpoints.GetFactions;

public static class GetFactionsEndpoint
{
    public static async Task<Ok<FactionResponse>> ExecuteAsync(
        int expressionId,
        IGetFactionsUseCase createFactionUseCase
    )
    {
        var results = await createFactionUseCase.ExecuteAsync(
            new GetFactionsModel() { ExpressionId = expressionId }
        );

        return TypedResults.Ok(
            new FactionResponse()
            {
                Factions = results
                    .Value.Factions.OrderBy(x => x.Name)
                    .Select(x => new FactionViewModel()
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
