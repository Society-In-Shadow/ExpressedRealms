using ExpressedRealms.Knowledges.UseCases.Knowledges.GetEditKnowledge;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.GetKnowledge;

public static class GetKnowledgeEndpoint
{
    public static async Task<Ok<GetKnowledgeResponse>> GetKnowledge(
        int id,
        IGetEditKnowledgeUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new GetEditKnowledgeModel() { Id = id }
        );

        return TypedResults.Ok(
            new GetKnowledgeResponse()
            {
                Id = results.Value.Id,
                Name = results.Value.Name,
                Description = results.Value.Description,
                TypeId = results.Value.KnowledgeTypeId,
            }
        );
    }
}
