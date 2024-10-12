using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Server.Configuration.UserRoles;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Requests;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints;

internal static class ExpressionEndpoints
{
    internal static void AddExpressionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "{name}",
                async (string name, ExpressedRealmsDbContext dbContext) =>
                {
                    var sections = await dbContext
                        .ExpressionSections.AsNoTracking()
                        .Where(x => x.Expression.Name.ToLower() == name.ToLower())
                        .ToListAsync();

                    return TypedResults.Ok(BuildExpressionPage(sections, null));
                }
            )
            .RequireAuthorization();
        
        endpointGroup
            .MapGet(
                "{expressionId}",
                [Authorize(Roles = UserRoles.ExpressionEditor)]
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
            )
            .RequireAuthorization();
        
        endpointGroup
            .MapPut(
                "{expressionId}",
                [Authorize(Roles = UserRoles.ExpressionEditor)]
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
                            NavMenuImage = editExpressionRequest.NavMenuImage
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
            .WithDescription(
                "You will also be able to set the publish status of the expression."
            )
            .RequireAuthorization();
        
        endpointGroup
            .MapPost("",
                [Authorize(Roles = UserRoles.ExpressionEditor)]
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
                            NavMenuImage = request.NavMenuImage
                        }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Created("/expressions", results.Value);
                }
            )
            .WithSummary("Allows one to create new expressions")
            .RequireAuthorization();
        
        endpointGroup
            .MapDelete(
                "{id}",
                [Authorize(Roles = UserRoles.ExpressionEditor)]
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
            )
            .RequireAuthorization();
    }

    private static List<ExpressionSectionDTO> BuildExpressionPage(
        List<ExpressionSection> dbSections,
        int? parentId
    )
    {
        List<ExpressionSectionDTO> sections = new();

        var filteredSections = dbSections
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Id)
            .ToList();
        foreach (var dbSection in filteredSections)
        {
            var dto = new ExpressionSectionDTO()
            {
                Name = dbSection.Name,
                Id = dbSection.Id,
                Content = dbSection.Content,
            };

            if (dbSections.Any(x => x.ParentId == dbSection.Id))
            {
                dto.SubSections = BuildExpressionPage(dbSections, dbSection.Id);
            }

            sections.Add(dto);
        }

        return sections;
    }
}
