using ExpressedRealms.Authentication;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Create;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Delete;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using EditProgressionPathEndpoint = ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Edit.EditProgressionPathEndpoint;

namespace ExpressedRealms.Expressions.API.ProgressionPaths;

internal static class ProgressionEndpoints
{
    internal static void AddProgressionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expression Progressions")
            .WithOpenApi();

        endpointGroup
            .MapGet("{expressionId}/progressions", GetProgressionPathsEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy)
            .WithSummary("Returns the high level information for a given expression")
            .WithDescription(
                "This returns the detailed information for the given expression, including publish details"
            );

        endpointGroup
            .MapPut("{expressionId}/progressions/{progressionId}", EditProgressionPathEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy)
            .WithSummary("Allows one to edit the high level expression details")
            .WithDescription("You will also be able to set the publish status of the expression.");

        endpointGroup
            .MapPost("{expressionId}/progressions/{progressionId}", CreateProgressionPathEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy)
            .WithSummary("Allows one to create new expressions");

        endpointGroup.MapDelete("{expressionId}/progressions/{progressionId}", DeleteProgressionPathEndpoint.ExecuteAsync);
    }
}
