using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.CreateExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.DeleteExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.EditExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.GetEditExpression;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.GetExpressionBooklet;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.GetExpressionCmsReport;
using ExpressedRealms.Expressions.API.ExpressionEndpoints.UpdateHierarchy;
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
            .WithTags("Expressions");

        // The following endpoints are checking for permissions internally
        // Permissions are dependent on the expression type id, not expression id
        endpointGroup.MapGet("{expressionId}", GetEditExpressionEndpoint.GetEditExpression);

        endpointGroup.MapPut("{expressionId}", EditExpressionEndpoint.EditExpression);

        endpointGroup.MapPut("{expressionId}/updateHierarchy", UpdateHierarchyEndpoint.UpdateHierarchy)
            .WithDescription(
                "This is an all or nothing operation.  It needs to be called with all the items, not a subset of them."
            );

        endpointGroup.MapPost("", CreateExpressionEndpoint.CreateExpression);

        endpointGroup.MapDelete("{id}", DeleteExpressionEndpoint.DeleteExpression);
        
        endpointGroup
            .MapGet("{expressionId}/report", GetExpressionCmsReportEndpoint.GetExpressionCmsReport)
            .RequirePermission(Permissions.ContentManagementSystem.DownloadReport);

        endpointGroup
            .MapGet("{expressionId}/booklet", GetExpressionBookletEndpoint.GetExpressionBooklet)
            .RequirePermission(Permissions.Expression.DownloadBooklet);
    }
}
