using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Create;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Delete;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Edit;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Create;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Delete;
using ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Get;
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
            .WithTags("Expression Progressions");

        endpointGroup
            .MapGet("{expressionId}/progressions", GetProgressionPathsEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.ProgressionPath.View);

        endpointGroup
            .MapPut(
                "{expressionId}/progressions/{progressionId}",
                EditProgressionPathEndpoint.ExecuteAsync
            )
            .RequirePermission(Permissions.ProgressionPath.Edit);

        endpointGroup
            .MapPost("{expressionId}/progressions/", CreateProgressionPathEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.ProgressionPath.Create);

        endpointGroup
            .MapDelete(
                "{expressionId}/progressions/{progressionId}",
                DeleteProgressionPathEndpoint.ExecuteAsync
            )
            .RequirePermission(Permissions.ProgressionPath.Delete);

        endpointGroup
            .MapPut(
                "{expressionId}/progressions/{progressionId}/levels/{levelId}",
                EditProgressionLevelEndpoint.ExecuteAsync
            )
            .RequirePermission(Permissions.ProgressionPath.Edit);

        endpointGroup
            .MapPost(
                "{expressionId}/progressions/{progressionId}/levels",
                CreateProgressionLevelEndpoint.ExecuteAsync
            )
            .RequirePermission(Permissions.ProgressionPath.Create);

        endpointGroup
            .MapDelete(
                "{expressionId}/progressions/{progressionId}/levels/{levelId}",
                DeleteProgressionLevelEndpoint.ExecuteAsync
            )
            .RequirePermission(Permissions.ProgressionPath.Delete);
    }
}
