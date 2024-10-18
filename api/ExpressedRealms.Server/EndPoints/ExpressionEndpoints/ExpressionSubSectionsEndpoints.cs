using ExpressedRealms.Authentication;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Helpers;
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
                "{name}",
                async Task<Results<NotFound, Ok<ExpressionBaseResponse>>>
                    (string name, HttpContext httpContext, IExpressionTextSectionRepository repository) =>
                {
                    var expressionIdResult = await repository.GetExpressionId(name);
                    if (expressionIdResult.HasNotFound(out var notFound))
                        return notFound;
                    expressionIdResult.ThrowIfErrorNotHandled();
                    
                    var sections = await repository.GetExpressionTextSections(expressionIdResult.Value);
                    
                    var hasEditPolicy = await httpContext.UserHasPolicyAsync(
                        Policies.ExpressionEditorPolicy
                    );
                    
                    return TypedResults.Ok( new ExpressionBaseResponse()
                    {
                        ExpressionId = expressionIdResult.Value,
                        ExpressionSections = ExpressionHelpers.BuildExpressionPage(sections),
                        CanEditPolicy = hasEditPolicy
                    });
                }
            )
            .RequireAuthorization();
        
        endpointGroup
            .MapGet(
                "{expressionId}/{sectionId}",
                async Task<Results<NotFound, Ok<EditExpressionSectionResponse>>>
                    (int expressionId, int sectionId, IExpressionTextSectionRepository repository) =>
                {
                    var sectionResult = await repository.GetExpressionTextSection(sectionId);
                    
                    if (sectionResult.HasNotFound(out var sectionNotFound))
                        return sectionNotFound;
                    sectionResult.ThrowIfErrorNotHandled();
                    
                    return TypedResults.Ok(new EditExpressionSectionResponse()
                    {
                        Id = sectionResult.Value.Id,
                        Name = sectionResult.Value.Name,
                        Content = sectionResult.Value.Content,
                        ParentId = sectionResult.Value.ParentId,
                        SectionTypeId = sectionResult.Value.SectionTypeId
                    });
                }
            )
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);
        
        endpointGroup
            .MapGet(
                "{expressionId}/{sectionId}/options",
                async Task<Results<NotFound, Ok<ExpressionSectionOptionsResponse>>>
                    (int expressionId, int sectionId, IExpressionTextSectionRepository repository) =>
                {
                    var optionsResult = await repository.GetExpressionTextSectionOptions(new GetExpressionTestSectionOptionsDto()
                    {
                        ExpressionId = expressionId,
                        SectionId = sectionId == 0 ? null : sectionId // Handle Create (0 = null)
                    });
                    
                    if (optionsResult.HasNotFound(out var notFound))
                        return notFound;
                    optionsResult.ThrowIfErrorNotHandled();
                    
                    return TypedResults.Ok(new ExpressionSectionOptionsResponse()
                    {
                        SectionTypes = optionsResult.Value.ExpressionSectionTypes.Select(x => new SectionTypeDto()
                        {
                            Id = x.Id, 
                            Name = x.Name, 
                            Description = x.Description
                        }).ToList(),
                        AvailableParents = ExpressionHelpers.BuildAvailableParentTree(optionsResult.Value.AvailableParents)
                        
                    });
                }
            )
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);
    }
}
