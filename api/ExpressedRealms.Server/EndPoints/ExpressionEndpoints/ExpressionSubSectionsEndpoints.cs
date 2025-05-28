using ExpressedRealms.Authentication;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Helpers;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Requests;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using SectionTypeDto = ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs.SectionTypeDto;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints;

internal static class ExpectedSubSectionsEndpoints
{
    internal static void AddExpressionSubsectionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expressionSubSections")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "{name}/expression",
                async Task<Results<NotFound, Ok<ExpressionSectionDTO>>> (
                    string name,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var expressionIdResult = await repository.GetExpressionId(name);
                    if (expressionIdResult.HasNotFound(out var notFound))
                        return notFound;
                    expressionIdResult.ThrowIfErrorNotHandled();

                    var section = await repository.GetExpressionSection(expressionIdResult.Value);

                    return TypedResults.Ok(
                        new ExpressionSectionDTO()
                        {
                            Name = section.Name,
                            Id = section.Id,
                            Content = section.Content,
                            SubSections = new List<ExpressionSectionDTO>(),
                        }
                    );
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{name}",
                async Task<Results<NotFound, Ok<ExpressionBaseResponse>>> (
                    string name,
                    HttpContext httpContext,
                    IExpressionTextSectionRepository repository,
                    IFeatureToggleClient featureToggleClient
                ) =>
                {
                    var expressionIdResult = await repository.GetExpressionId(name);
                    if (expressionIdResult.HasNotFound(out var notFound))
                        return notFound;
                    expressionIdResult.ThrowIfErrorNotHandled();

                    var sections = await repository.GetExpressionTextSections(
                        expressionIdResult.Value
                    );

                    var hasEditPolicy = await httpContext.UserHasPolicyAsync(
                        Policies.ExpressionEditorPolicy
                    );

                    return TypedResults.Ok(
                        new ExpressionBaseResponse()
                        {
                            ExpressionId = expressionIdResult.Value,
                            ExpressionSections = ExpressionHelpers.BuildExpressionPage(sections),
                            CanEditPolicy = hasEditPolicy,
                            ShowPowersTab = await featureToggleClient.HasFeatureFlag(
                                ReleaseFlags.ShowPowersTab
                            ),
                        }
                    );
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{expressionId}/{sectionId}",
                async Task<Results<NotFound, Ok<EditExpressionSectionResponse>>> (
                    int expressionId,
                    int sectionId,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var sectionResult = await repository.GetExpressionTextSection(sectionId);

                    if (sectionResult.HasNotFound(out var sectionNotFound))
                        return sectionNotFound;
                    sectionResult.ThrowIfErrorNotHandled();

                    return TypedResults.Ok(
                        new EditExpressionSectionResponse()
                        {
                            Name = sectionResult.Value.Name,
                            Content = sectionResult.Value.Content,
                            ParentId = sectionResult.Value.ParentId,
                            SectionTypeId = sectionResult.Value.SectionTypeId,
                        }
                    );
                }
            )
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapGet(
                "{expressionId}/{sectionId}/options",
                async Task<Results<ValidationProblem, Ok<ExpressionSectionOptionsResponse>>> (
                    int expressionId,
                    int sectionId,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var optionsResult = await repository.GetExpressionTextSectionOptions(
                        new GetExpressionTestSectionOptionsDto()
                        {
                            ExpressionId = expressionId,
                            SectionId = sectionId == 0 ? null : sectionId, // Handle Create (0 = null)
                        }
                    );

                    if (optionsResult.HasValidationError(out var validationProblem))
                        return validationProblem;
                    optionsResult.ThrowIfErrorNotHandled();

                    return TypedResults.Ok(
                        new ExpressionSectionOptionsResponse()
                        {
                            SectionTypes = optionsResult
                                .Value.ExpressionSectionTypes.Select(x => new SectionTypeDto()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Description = x.Description,
                                })
                                .ToList(),
                            AvailableParents = ExpressionHelpers.BuildAvailableParentTree(
                                optionsResult.Value.AvailableParents
                            ),
                        }
                    );
                }
            )
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapPut(
                "{expressionId}/{sectionId}",
                async Task<Results<NotFound, ValidationProblem, NoContent>> (
                    int expressionId,
                    int sectionId,
                    EditExpressionSubSectionTextRequest request,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var results = await repository.EditExpressionTextSectionAsync(
                        new EditExpressionTextSectionDto()
                        {
                            Id = sectionId,
                            ExpressionId = expressionId,
                            Name = request.Name,
                            Content = request.Content,
                            SectionTypeId = request.SectionTypeId,
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
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapPost(
                "{expressionId}",
                async Task<Results<NotFound, ValidationProblem, Created<int>>> (
                    int expressionId,
                    CreateExpressionSubSectionTextRequest request,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var results = await repository.CreateExpressionTextSectionAsync(
                        new CreateExpressionTextSectionDto()
                        {
                            ExpressionId = expressionId,
                            Name = request.Name,
                            Content = request.Content,
                            SectionTypeId = request.SectionTypeId,
                            ParentId = request.ParentId,
                        }
                    );

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Created("/", results.Value);
                }
            )
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapDelete(
                "{expressionId}/{sectionId}",
                async Task<Results<NotFound, StatusCodeHttpResult, NoContent>> (
                    int expressionId,
                    int sectionId,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var results = await repository.DeleteExpressionTextSectionAsync(
                        expressionId,
                        sectionId
                    );

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasBeenDeletedAlready(out var deletedAlready))
                        return deletedAlready;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);
    }
}
