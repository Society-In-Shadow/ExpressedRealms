using ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public static class GetAllBlessingsEndpoint
{
    public static async Task<Ok<GetAllBlessingsResponse>> GetBlessings(
        IGetBlessingsUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync();

        return TypedResults.Ok(
            new GetAllBlessingsResponse()
            {
                Advantages = GetDetailsFor(results, "Advantage"),
                Disadvantages = GetDetailsFor(results, "Disadvantage"),
                MixedBlessings = GetDetailsFor(results, "Mixed Blessing"),
            }
        );
    }

    private static List<Blessing> GetDetailsFor(
        Result<GetBlessingsReturnModel> results,
        string typeName
    )
    {
        return results
            .Value.Blessings.Where(x => x.Type == typeName)
            .OrderBy(x => x.Name)
            .Select(x => new Blessing()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                SubCategory = x.SubCategory,
                Levels = x
                    .Levels.OrderBy(y => y.Level)
                    .Select(y => new Level()
                    {
                        Id = y.Id,
                        Name = y.Level,
                        Description = y.Description,
                        XpCost = y.XpCost,
                        XpGain = y.XpGain,
                    })
                    .ToList(),
            })
            .ToList();
    }
}
