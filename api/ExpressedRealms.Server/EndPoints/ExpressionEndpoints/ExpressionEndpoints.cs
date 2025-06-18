using ExpressedRealms.Authentication;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Helpers;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Requests;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints;

internal static class ExpressionEndpoints
{
    internal static void AddExpressionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy)
            .WithTags("Expressions")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "{expressionId}",
                async Task<Results<NotFound, ValidationProblem, Ok<EditExpressionResponse>>> (
                    int expressionId,
                    IExpressionRepository repository
                ) =>
                {
                    var results = await repository.GetExpression(expressionId);

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Ok(new EditExpressionResponse(results.Value));
                }
            )
            .WithSummary("Returns the high level information for a given expression")
            .WithDescription(
                "This returns the detailed information for the given expression, including publish details"
            );

        endpointGroup
            .MapGet(
                "/getByName/{name}",
                async Task<Results<NotFound, Ok<ExpressionNameResponse>>> (
                    string name,
                    IExpressionTextSectionRepository sectionRepository,
                    IExpressionRepository expressionRepository,
                    IFeatureToggleClient featureToggleClient
                ) =>
                {
                    int expressionId;
                    if (name == "ruleBook")
                    {
                        var result = await expressionRepository.GetGameSystemExpressionId();
                        expressionId = result.Value;
                    }
                    else if (name == "treasuredTales")
                    {
                        var result = await expressionRepository.GetTreasuredTalesExpressionId();
                        expressionId = result.Value;
                    }
                    else
                    {
                        var expressionIdResult = await sectionRepository.GetExpressionId(name);
                        if (expressionIdResult.HasNotFound(out var notFound))
                            return notFound;
                        expressionIdResult.ThrowIfErrorNotHandled();
                        expressionId = expressionIdResult.Value;
                    }

                    return TypedResults.Ok(
                        new ExpressionNameResponse
                        {
                            Id = expressionId,
                            ShowPowersTab = await featureToggleClient.HasFeatureFlag(
                                ReleaseFlags.ShowPowersTab
                            ),
                        }
                    );
                }
            )
            .WithSummary("Returns the id for the given expression name");

        endpointGroup
            .MapPut(
                "{expressionId}",
                async Task<Results<NotFound, ValidationProblem, NoContent>> (
                    int expressionId,
                    EditExpressionRequest editExpressionRequest,
                    IExpressionRepository repository
                ) =>
                {
                    var results = await repository.EditExpressionAsync(
                        new EditExpressionDto()
                        {
                            Id = editExpressionRequest.Id,
                            Name = editExpressionRequest.Name,
                            PublishStatus = editExpressionRequest.PublishStatus,
                            ShortDescription = editExpressionRequest.ShortDescription,
                            NavMenuImage = editExpressionRequest.NavMenuImage,
                        }
                    );

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .WithSummary("Allows one to edit the high level expression details")
            .WithDescription("You will also be able to set the publish status of the expression.");

        endpointGroup
            .MapPut(
                "{expressionId}/updateHierarchy",
                async Task<Results<NotFound, ValidationProblem, NoContent>> (
                    int expressionId,
                    EditExpressionHierarchyItemRequest editExpressionRequest,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var results = await repository.UpdateSectionHierarchyAndSorting(
                        new EditExpressionHierarchyDto()
                        {
                            ExpressionId = expressionId,
                            Items = ExpressionHelpers.FlattenHierarchy(editExpressionRequest.Items),
                        }
                    );

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .WithSummary("Allows one to modify the hierarchy of the expression")
            .WithDescription(
                "This is an all or nothing operation.  It needs to be called with all the items, not a subset of them."
            );

        endpointGroup
            .MapPost(
                "",
                async Task<Results<ValidationProblem, Created<int>>> (
                    AddExpressionRequest request,
                    IExpressionRepository repository
                ) =>
                {
                    var results = await repository.CreateExpressionAsync(
                        new CreateExpressionDto()
                        {
                            Name = request.Name,
                            ShortDescription = request.ShortDescription,
                            NavMenuImage = request.NavMenuImage,
                        }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Created("/expressions", results.Value);
                }
            )
            .WithSummary("Allows one to create new expressions");

        endpointGroup.MapDelete(
            "{id}",
            async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> (
                int id,
                IExpressionRepository repository
            ) =>
            {
                var status = await repository.DeleteExpressionAsync(id);

                if (status.HasNotFound(out var notFound))
                    return notFound;
                if (status.HasBeenDeletedAlready(out var deletedAlready))
                    return deletedAlready;
                status.ThrowIfErrorNotHandled();

                return TypedResults.NoContent();
            }
        );
    }
}
