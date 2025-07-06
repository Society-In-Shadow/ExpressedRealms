using ExpressedRealms.Authentication;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Requests;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;
using ExpressedRealms.Expressions.API.Helpers;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using ExpressionSectionDto = ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs.ExpressionSectionDto;
using SectionTypeDto = ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs.SectionTypeDto;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints;

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
                "{id}/expression",
                async Task<Results<NotFound, Ok<ExpressionSectionDto>>> (
                    int id,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var section = await repository.GetExpressionSection(id);

                    if (section is null)
                        return TypedResults.NotFound();

                    return TypedResults.Ok(
                        new ExpressionSectionDto()
                        {
                            Name = section.Name,
                            Id = section.Id,
                            Content = section.Content,
                            SubSections = new List<ExpressionSectionDto>(),
                        }
                    );
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{id}",
                async Task<Results<NotFound, Ok<ExpressionBaseResponse>>> (
                    int id,
                    IExpressionTextSectionRepository repository
                ) =>
                {
                    var sections = await repository.GetExpressionTextSections(id);

                    return TypedResults.Ok(
                        new ExpressionBaseResponse()
                        {
                            ExpressionSections = ExpressionHelpers.BuildExpressionPage(sections),
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
