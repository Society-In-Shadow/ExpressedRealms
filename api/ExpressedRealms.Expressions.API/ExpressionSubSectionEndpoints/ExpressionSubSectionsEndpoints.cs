using ExpressedRealms.Authentication;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.CreateExpressionSubSectionText;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DeleteExpressionSubSectionText;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.EditExpressionSubSectionText;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSection;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSectionOptions;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionTextSection;
using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionTextSections;
using ExpressedRealms.Server.Shared;
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

        endpointGroup
            .MapGet("{expressionId}/{sectionId}", GetExpressionTextSectionEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapGet("{expressionId}/{sectionId}/options", GetExpressionSectionOptionsEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapPut("{expressionId}/{sectionId}", EditExpressionSubSectionTextEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapPost("{expressionId}", CreateExpressionSubSectionTextEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);

        endpointGroup
            .MapDelete("{expressionId}/{sectionId}", DeleteExpressionSubSectionTextEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ExpressionEditorPolicy);
    }
}