using ExpressedRealms.Knowledges.API.GetAllExpressions;
using ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledges;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.GetAllKnowledges;

public static class GetKnowledgesEndpoint
{
    public static async Task<Ok<KnowledgeResponse>> GetKnowledges(
        IGetKnowledgesUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync();

        return TypedResults.Ok(
            new KnowledgeResponse()
            {
                Knowledges = results
                    .Value.KnowledgeTypes.Select(x => new KnowledgeViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        TypeName = x.TypeName,
                        TypeDescription = x.TypeDescription,
                        TypeId = x.TypeId,
                    })
                    .ToList(),
            }
        );
    }
}
