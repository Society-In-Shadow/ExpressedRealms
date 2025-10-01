using ExpressedRealms.Authentication;
using ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList;
using ExpressedRealms.Powers.API.PowerPathEndpoints.Requests;
using ExpressedRealms.Powers.API.PowerPathEndpoints.Responses.PowerPathList;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathSorting;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Powers.UseCases.GetPowerBookletReport;
using ExpressedRealms.Powers.UseCases.GetPowerCardReport;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using DetailedInformation = ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList.DetailedInformation;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints;

internal static class PowerPathEndpoints
{
    internal static void AddPowerPathApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powerpath")
            .AddFluentValidationAutoValidation()
            .WithTags("Power Paths")
            .WithOpenApi();

        app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi()
            .MapGet(
                "/{expressionId}/powerPaths",
                async (int expressionId, IPowerPathRepository powerRepository) =>
                {
                    var powers = await powerRepository.GetPowerPathAndPowers(expressionId);

                    return TypedResults.Ok(
                        powers.Value.Select(x => new PowerPathInformationResponse()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            Powers = x
                                .Powers.Select(y => new PowerInformationResponse()
                                {
                                    Id = y.Id,
                                    Name = y.Name,
                                    Category =
                                        y.Category?.Select(z => new DetailedInformation(z)).ToList()
                                        ?? new List<DetailedInformation>(),
                                    Description = y.Description,
                                    GameMechanicEffect = y.GameMechanicEffect,
                                    Limitation = y.Limitation,
                                    PowerDuration = new DetailedInformation(y.PowerDuration),
                                    AreaOfEffect = new DetailedInformation(y.AreaOfEffect),
                                    PowerLevel = new DetailedInformation(y.PowerLevel),
                                    PowerActivationType = new DetailedInformation(
                                        y.PowerActivationType
                                    ),
                                    Other = y.Other,
                                    IsPowerUse = y.IsPowerUse,
                                    Cost = y.Cost,
                                    Prerequisites = y.Prerequisites is not null
                                        ? new PrerequisiteDetails()
                                        {
                                            RequiredAmount = y.Prerequisites.RequiredAmount,
                                            Powers = y.Prerequisites.Powers,
                                        }
                                        : null,
                                    ModifierGroupId = y.ModifierGroupId,
                                })
                                .ToList(),
                        })
                    );
                }
            )
            .WithSummary("Returns the list of power paths for a given expression")
            .RequireAuthorization();

        app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi()
            .MapGet(
                "/{expressionId}/getPowerCards",
                async Task<FileStreamHttpResult> (
                    int expressionId,
                    bool isFiveByThree,
                    IGetPowerCardReportUseCase useCase
                ) =>
                {
                    var reportStream = await useCase.ExecuteAsync(
                        new GetPowerCardReportModel()
                        {
                            ExpressionId = expressionId,
                            IsFiveByThree = isFiveByThree,
                        }
                    );

                    return TypedResults.File(
                        reportStream,
                        contentType: "application/pdf",
                        fileDownloadName: "powerCardReport.pdf",
                        enableRangeProcessing: true
                    );
                }
            )
            .WithSummary("Downloads the power cards for the given expression")
            .RequireAuthorization();

        app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi()
            .MapGet(
                "/{expressionId}/powerBooklet",
                async Task<FileStreamHttpResult> (
                    int expressionId,
                    IGetPowerBookletReportUseCase useCase
                ) =>
                {
                    var reportStream = await useCase.ExecuteAsync(
                        new GetPowerBookletReportModel() { ExpressionId = expressionId }
                    );

                    return TypedResults.File(
                        reportStream.Value,
                        contentType: "application/pdf",
                        fileDownloadName: "powerBookletReport.pdf",
                        enableRangeProcessing: true
                    );
                }
            )
            .WithSummary("Downloads the power cards for the given expression")
            .RequireAuthorization();

        app.MapGroup("expression")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi()
            .MapPut(
                "/{expressionId}/updateSorting",
                async (
                    int expressionId,
                    PowerPathOrderUpdateRequest request,
                    IPowerPathRepository powerRepository
                ) =>
                {
                    await powerRepository.UpdatePowerPathSortOrder(
                        new EditPowerPathSortModel()
                        {
                            ExpressionId = expressionId,
                            Items = request
                                .Items.Select(x => new EditPowerPathSortItemModel()
                                {
                                    Id = x.Id,
                                    SortOrder = x.SortOrder,
                                })
                                .ToList(),
                        }
                    );

                    return TypedResults.Ok();
                }
            )
            .WithSummary("Updates the sort order of power paths for a given expression")
            .RequirePolicyAuthorization(Policies.ManagePowers);

        endpointGroup
            .MapGet(
                "/{id}",
                async Task<Results<NotFound, Ok<EditPowerPathResponse>>> (
                    int id,
                    IPowerPathRepository powerRepository
                ) =>
                {
                    var powers = await powerRepository.GetPowerPathAsync(id);

                    if (powers.HasNotFound(out var notFound))
                        return notFound;

                    return TypedResults.Ok(
                        new EditPowerPathResponse()
                        {
                            Id = powers.Value.Id,
                            Name = powers.Value.Name,
                            Description = powers.Value.Description,
                        }
                    );
                }
            )
            .WithSummary(
                "Returns the specified power path for a given expression for editing purposes"
            )
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "",
                async Task<Results<ValidationProblem, NotFound, Created<int>>> (
                    CreatePowerPathRequest request,
                    IPowerPathRepository repository
                ) =>
                {
                    var results = await repository.CreatePowerPathAsync(
                        new CreatePowerPathModel()
                        {
                            Name = request.Name,
                            Description = request.Description,
                            ExpressionId = request.ExpressionId,
                        }
                    );

                    if (results.HasNotFound(out var notFound))
                    {
                        return notFound;
                    }
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Created("/powers", results.Value);
                }
            )
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Allows one to create new power paths");

        endpointGroup
            .MapPut(
                "{id}",
                async Task<Results<ValidationProblem, NotFound, NoContent>> (
                    int id,
                    EditPowerPathRequest request,
                    IPowerPathRepository repository
                ) =>
                {
                    var results = await repository.EditPowerPathAsync(
                        new EditPowerPathModel()
                        {
                            Id = request.Id,
                            Name = request.Name,
                            Description = request.Description,
                        }
                    );

                    if (results.HasNotFound(out var notFound))
                    {
                        return notFound;
                    }
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Allows one to edit new power paths");

        endpointGroup
            .MapDelete(
                "{id}",
                async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> (
                    int id,
                    IPowerPathRepository repository
                ) =>
                {
                    var status = await repository.DeletePowerPathAsync(id);

                    if (status.HasNotFound(out var notFound))
                        return notFound;
                    if (status.HasBeenDeletedAlready(out var deletedAlready))
                        return deletedAlready;
                    status.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .RequirePolicyAuthorization(Policies.ManagePowers);
    }
}
