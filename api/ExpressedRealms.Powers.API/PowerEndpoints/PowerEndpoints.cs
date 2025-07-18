using ExpressedRealms.Authentication;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Powers.API.PowerEndpoints.Requests.CreatePower;
using ExpressedRealms.Powers.API.PowerEndpoints.Requests.PowerEdit;
using ExpressedRealms.Powers.API.PowerEndpoints.Requests.UpdateOrder;
using ExpressedRealms.Powers.API.PowerEndpoints.Responses.Options;
using ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using DetailedInformation = ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList.DetailedInformation;

namespace ExpressedRealms.Powers.API.PowerEndpoints;

internal static class PowerEndpoints
{
    internal static void AddPowerApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powers")
            .AddFluentValidationAutoValidation()
            .RequireFeatureToggle(ReleaseFlags.ShowPowersTab)
            .WithTags("Powers")
            .WithOpenApi();

        app.MapGroup("/powerpath/")
            .AddFluentValidationAutoValidation()
            .RequireFeatureToggle(ReleaseFlags.ShowPowersTab)
            .WithTags("Power Paths")
            .WithOpenApi()
            .MapGet(
                "/{id}/powers",
                async (int id, IPowerRepository powerRepository) =>
                {
                    var powers = await powerRepository.GetPowersAsync(id);

                    return TypedResults.Ok(
                        powers.Value.Select(x => new PowerInformationResponse()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Category =
                                x.Category?.Select(x => new DetailedInformation(x)).ToList()
                                ?? new List<DetailedInformation>(),
                            Description = x.Description,
                            GameMechanicEffect = x.GameMechanicEffect,
                            Limitation = x.Limitation,
                            PowerDuration = new DetailedInformation(x.PowerDuration),
                            AreaOfEffect = new DetailedInformation(x.AreaOfEffect),
                            PowerLevel = new DetailedInformation(x.PowerLevel),
                            PowerActivationType = new DetailedInformation(x.PowerActivationType),
                            Other = x.Other,
                            IsPowerUse = x.IsPowerUse,
                            Prerequisites = x.Prerequisites is not null
                                ? new PrerequisiteDetails()
                                {
                                    RequiredAmount = x.Prerequisites.RequiredAmount,
                                    Powers = x.Prerequisites.Powers,
                                }
                                : null,
                            Cost = x.Cost,
                        })
                    );
                }
            )
            .WithSummary("Returns the list of powers for a given power path")
            .WithDescription(" of powers for a given power path")
            .RequireAuthorization();

        app.MapGroup("powerpath")
            .AddFluentValidationAutoValidation()
            .WithTags("Power Paths")
            .WithOpenApi()
            .MapPut(
                "/{powerPathId}/updateSorting",
                async (
                    int powerPathId,
                    PowerOrderUpdateRequest request,
                    IPowerRepository powerRepository
                ) =>
                {
                    await powerRepository.UpdatePowerPathSortOrder(
                        new EditPowerSortModel()
                        {
                            PowerPathId = powerPathId,
                            Items = request
                                .Items.Select(x => new EditPowerSortItemModel()
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
                "/{powerId}",
                async Task<Results<NotFound, Ok<EditPowerInformationResponse>>> (
                    int powerId,
                    IPowerRepository powerRepository
                ) =>
                {
                    var powers = await powerRepository.GetPowerAsync(powerId);

                    if (powers.HasNotFound(out var notFound))
                        return notFound;

                    return TypedResults.Ok(
                        new EditPowerInformationResponse()
                        {
                            Id = powers.Value.Id,
                            Name = powers.Value.Name,
                            CategoryIds = powers.Value.CategoryIds,
                            Description = powers.Value.Description,
                            GameMechanicEffect = powers.Value.GameMechanicEffect,
                            Limitation = powers.Value.Limitation,
                            PowerDurationId = powers.Value.PowerDurationId,
                            AreaOfEffectId = powers.Value.AreaOfEffectId,
                            PowerLevelId = powers.Value.PowerLevelId,
                            PowerActivationTypeId = powers.Value.PowerActivationTypeId,
                            Other = powers.Value.Other,
                            IsPowerUse = powers.Value.IsPowerUse,
                            Cost = powers.Value.Cost,
                        }
                    );
                }
            )
            .WithSummary("Returns the specified power for a given expression for editing purposes")
            .WithDescription(" of powers for a given expression")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "/options",
                async (IPowerRepository powerRepository) =>
                {
                    var options = await powerRepository.GetPowerOptionsAsync();

                    return TypedResults.Ok(
                        new PowerOptionsResponse()
                        {
                            Category = options
                                .Value.Category.Select(x => new DetailedEditInformation(x))
                                .ToList(),
                            PowerDuration = options
                                .Value.PowerDuration.Select(x => new DetailedEditInformation(x))
                                .ToList(),
                            PowerLevel = options
                                .Value.PowerLevel.Select(x => new DetailedEditInformation(x))
                                .ToList(),
                            AreaOfEffect = options
                                .Value.AreaOfEffect.Select(x => new DetailedEditInformation(x))
                                .ToList(),
                            PowerActivationType = options
                                .Value.PowerActivationType.Select(x => new DetailedEditInformation(
                                    x
                                ))
                                .ToList(),
                        }
                    );
                }
            )
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Returns available options for powers")
            .WithDescription(
                "This endpoint retrieves the available options for creating or editing powers."
            );

        endpointGroup
            .MapPost(
                "",
                async Task<Results<ValidationProblem, NotFound, Created<int>>> (
                    CreatePowerRequest request,
                    IPowerRepository repository
                ) =>
                {
                    var results = await repository.CreatePower(
                        new CreatePowerModel()
                        {
                            Name = request.Name,
                            Category = request.CategoryIds,
                            Description = request.Description,
                            GameMechanicEffect = request.GameMechanicEffect,
                            Limitation = request.Limitation,
                            PowerDuration = request.PowerDuration,
                            AreaOfEffect = request.AreaOfEffect,
                            PowerLevel = request.PowerLevel,
                            PowerActivationType = request.PowerActivationType,
                            Other = request.Other,
                            PowerPathId = request.PowerPathId,
                            IsPowerUse = request.IsPowerUse,
                            Cost = request.Cost,
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
            .WithSummary("Allows one to create new powers");

        endpointGroup
            .MapPut(
                "{id}",
                async Task<Results<ValidationProblem, NotFound, NoContent>> (
                    int id,
                    EditPowerRequest request,
                    IPowerRepository repository
                ) =>
                {
                    var results = await repository.EditPower(
                        new EditPowerModel()
                        {
                            Id = request.Id,
                            Name = request.Name,
                            Category = request.CategoryIds,
                            Description = request.Description,
                            GameMechanicEffect = request.GameMechanicEffect,
                            Limitation = request.Limitation,
                            PowerDuration = request.PowerDurationId,
                            AreaOfEffect = request.AreaOfEffectId,
                            PowerLevel = request.PowerLevelId,
                            PowerActivationType = request.PowerActivationTypeId,
                            Other = request.Other,
                            IsPowerUse = request.IsPowerUse,
                            Cost = request.Cost,
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
            .WithSummary("Allows one to edit new powers");

        endpointGroup
            .MapDelete(
                "{id}",
                async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> (
                    int id,
                    IPowerRepository repository
                ) =>
                {
                    var status = await repository.DeletePowerAsync(id);

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
