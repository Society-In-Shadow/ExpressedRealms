using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.CreateExpressionSubSectionText;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DeleteExpressionSubSectionText;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.EditExpressionSubSectionText;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSection;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSectionOptions;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionTextSection;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionTextSections;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints;

internal static class ExpectedSubSectionsEndpoints
{
    internal static void AddExpressionSubsectionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expressionSubSections")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions");

        endpointGroup
            .MapGet("{id}/expression", GetExpressionSectionEndpoint.ExecuteAsync)
            .RequireAuthorization();

        endpointGroup
            .MapGet("{id}", GetExpressionTextSectionsEndpoint.ExecuteAsync)
            .RequireAuthorization();

        // These permissions are done on the use case level
        endpointGroup.MapGet("{expressionId}/{sectionId}", GetExpressionTextSectionEndpoint.ExecuteAsync);

        endpointGroup
            .MapGet("{expressionId}/{sectionId}/options", GetExpressionSectionOptionsEndpoint.ExecuteAsync);

        endpointGroup
            .MapPut("{expressionId}/{sectionId}", EditExpressionSubSectionTextEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("{expressionId}", CreateExpressionSubSectionTextEndpoint.ExecuteAsync);

        endpointGroup
            .MapDelete("{expressionId}/{sectionId}", DeleteExpressionSubSectionTextEndpoint.ExecuteAsync);
    }
}