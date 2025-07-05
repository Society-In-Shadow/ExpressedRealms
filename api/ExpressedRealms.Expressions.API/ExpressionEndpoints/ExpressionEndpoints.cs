using ExpressedRealms.Authentication;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.CreateExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.DeleteExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.EditExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.GetEditExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.GetExpressionId;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.UpdateHierarchy;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints;

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
            .MapGet("{expressionId}", GetEditExpressionEndpoint.GetEditExpression)
            .WithSummary("Returns the high level information for a given expression")
            .WithDescription(
                "This returns the detailed information for the given expression, including publish details"
            );

        endpointGroup
            .MapGet("/getByName/{name}", GetExpressionIdByNameEndpoint.GetExpressionIdByName)
            .WithSummary("Returns the id for the given expression name");

        endpointGroup
            .MapPut("{expressionId}", EditExpressionEndpoint.EditExpression)
            .WithSummary("Allows one to edit the high level expression details")
            .WithDescription("You will also be able to set the publish status of the expression.");

        endpointGroup
            .MapPut("{expressionId}/updateHierarchy", UpdateHierarchyEndpoint.UpdateHierarchy)
            .WithSummary("Allows one to modify the hierarchy of the expression")
            .WithDescription(
                "This is an all or nothing operation.  It needs to be called with all the items, not a subset of them."
            );

        endpointGroup
            .MapPost("", CreateExpressionEndpoint.CreateExpression)
            .WithSummary("Allows one to create new expressions");

        endpointGroup.MapDelete("{id}", DeleteExpressionEndpoint.DeleteExpression);
    }
}
