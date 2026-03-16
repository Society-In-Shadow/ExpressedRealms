using ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinQuestions;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetCheckinQuestions;

public static class GetCheckinQuestionsEndpoint
{
    public static async Task<
        Results<Ok<GetCheckinQuestionsResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(string lookupId, [FromServices] IGetCheckinQuestionsUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new() { LookupId = lookupId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetCheckinQuestionsResponse()
            {
                HasCompletedStage = results.Value.HasCompletedStage,
                Questions = results
                    .Value.Questions.Select(x => new QuestionResponse()
                    {
                        Question = x.Question,
                        TypeId = x.QuestionTypeId,
                        Response = x.Response,
                        Id = x.QuestionId,
                    })
                    .ToList(),
            }
        );
    }
}
