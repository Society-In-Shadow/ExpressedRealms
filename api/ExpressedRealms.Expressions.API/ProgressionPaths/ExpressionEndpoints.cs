using ExpressedRealms.Authentication;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Create;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Delete;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Edit;
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
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);

        endpointGroup
            .MapPut(
                "{expressionId}/progressions/{progressionId}",
                EditProgressionPathEndpoint.ExecuteAsync
            )
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);

        endpointGroup
            .MapPost("{expressionId}/progressions/", CreateProgressionPathEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);

        endpointGroup
            .MapDelete(
                "{expressionId}/progressions/{progressionId}",
                DeleteProgressionPathEndpoint.ExecuteAsync
            )
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);

        endpointGroup
            .MapPut(
                "{expressionId}/progressions/{progressionId}/levels/{levelId}",
                EditProgressionLevelEndpoint.ExecuteAsync
            )
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);

        endpointGroup
            .MapPost(
                "{expressionId}/progressions/{progressionId}/levels",
                CreateProgressionLevelEndpoint.ExecuteAsync
            )
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);

        endpointGroup
            .MapDelete(
                "{expressionId}/progressions/{progressionId}/levels/{levelId}",
                DeleteProgressionLevelEndpoint.ExecuteAsync
            )
            .RequirePolicyAuthorization(Policies.ManageProgressionPaths);
    }
}
