using ExpressedRealms.Authentication;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Powers.API.PowerEndpoints.Responses.Options;
using ExpressedRealms.Powers.API.PowerPrerequisites.Requests.CreatePrerequisite;
using ExpressedRealms.Powers.API.PowerPrerequisites.Requests.EditPrerequisite;
using ExpressedRealms.Powers.API.PowerPrerequisites.Responses.GetPrerequisites;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DeletePrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Powers.API.PowerPrerequisites;

internal static class PowerPrerequisiteEndpoints
{
    internal static void AddPowerPrerequisiteApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powers")
            .AddFluentValidationAutoValidation()
            .RequireFeatureToggle(ReleaseFlags.ShowPowersTab)
            .WithTags("Powers")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "/{powerId}/prerequisites",
                async Task<Results<ValidationProblem, Ok<GetPrerequisiteResponse?>>> (
                    int powerId,
                    [FromServices] IGetPrerequisiteUseCase getCase
                ) =>
                {
                    var results = await getCase.ExecuteAsync(
                        new GetPrerequisiteModel() { PowerId = powerId }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    if (results.Value is null)
                    {
                        return TypedResults.Ok<GetPrerequisiteResponse?>(null);
                    }

                    return TypedResults.Ok<GetPrerequisiteResponse?>(
                        new GetPrerequisiteResponse()
                        {
                            Id = results.Value.Id,
                            RequiredAmount = results.Value.RequiredAmount,
                            PowerIds = results.Value.PowerIds,
                        }
                    );
                }
            )
            .WithSummary("Gets the prerequisite")
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "/{powerId}/prerequisite",
                async Task<Results<ValidationProblem, Ok>> (
                    int powerId,
                    CreatePrerequisiteRequest request,
                    [FromServices] ICreatePrerequisiteUseCase createCase
                ) =>
                {
                    var results = await createCase.ExecuteAsync(
                        new CreatePrerequisiteModel()
                        {
                            PowerId = powerId,
                            RequiredAmount = request.RequiredAmount,
                            PrerequisitePowerIds = request.PowerIds,
                        }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Ok();
                }
            )
            .WithSummary("Adds the prerequisite")
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .RequireAuthorization();

        endpointGroup
            .MapDelete(
                "/{powerId}/prerequisite/{prerequisiteId}",
                async Task<Results<ValidationProblem, Ok>> (
                    int powerId,
                    int prerequisiteId,
                    [FromServices] IDeletePrerequisiteUseCase deleteCase
                ) =>
                {
                    var results = await deleteCase.ExecuteAsync(
                        new DeletePrerequisiteModel() { Id = prerequisiteId }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Ok();
                }
            )
            .WithSummary("Deletes the prerequisite")
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "/{powerId}/prerequisite/{prerequisiteId}",
                async Task<Results<ValidationProblem, Ok>> (
                    int powerId,
                    int prerequisiteId,
                    EditPrerequisiteRequest request,
                    [FromServices] IEditPrerequisiteUseCase editCase
                ) =>
                {
                    var results = await editCase.ExecuteAsync(
                        new EditPrerequisiteModel()
                        {
                            Id = prerequisiteId,
                            RequiredAmount = request.RequiredAmount,
                            PrerequisitePowerIds = request.PowerIds,
                        }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Ok();
                }
            )
            .WithSummary("Edits the prerequisite")
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .RequireAuthorization();

        app.MapGroup("/powerpath/")
            .AddFluentValidationAutoValidation()
            .RequireFeatureToggle(ReleaseFlags.ShowPowersTab)
            .WithTags("Power Paths")
            .WithOpenApi()
            .MapGet(
                "/{id}/powerprerequisites/options",
                async (int id, IPowerRepository powerRepository) =>
                {
                    var powers = await powerRepository.GetPowersAsync(id);

                    var requiredAmount = new List<DetailedEditInformation>();

                    for (int i = 1; i <= powers.Value.Count - 1; i++)
                    {
                        requiredAmount.Add(
                            new DetailedEditInformation()
                            {
                                Id = i,
                                Name = GetName(i),
                                Description = GetName(i),
                            }
                        );
                    }

                    string GetName(int index)
                    {
                        if (index == 1 && index != powers.Value.Count - 1)
                            return "1 (Any)";

                        if (index == powers.Value.Count - 1)
                            return $"{index} (All)";

                        return $"{index}";
                    }

                    return TypedResults.Ok(
                        new
                        {
                            RequiredAmount = requiredAmount,
                            PrerequisitePowers = powers
                                .Value.Select(x => new DetailedEditInformation()
                                {
                                    Id = x.Id,
                                    Description = x.Description,
                                    Name = x.Name,
                                })
                                .ToList(),
                        }
                    );
                }
            )
            .WithSummary("Returns the list of powers for a given power path")
            .WithDescription(" of powers for a given power path")
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .RequireAuthorization();
    }
}
