using ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;
using ExpressedRealms.Powers.API.PowerEndpoints.Responses.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.GetKnowledgeTypes;

public static class GetKnowledgeTypesEndpoint
{
    public static async Task<Ok<KnowledgeTypeResponse>> GetKnowledgeTypes(
        IGetKnowledgeTypesUseCase getKnowledgeTypesUseCase
    )
    {
        var results = await getKnowledgeTypesUseCase.ExecuteAsync();

        return TypedResults.Ok(
            new KnowledgeTypeResponse()
            {
                KnowledgeTypes = results
                    .Value.KnowledgeTypes.Select(x => new DetailedEditInformation()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList(),
            }
        );
    }
}
