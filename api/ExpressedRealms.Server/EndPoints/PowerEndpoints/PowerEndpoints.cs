using ExpressedRealms.Authentication;
using ExpressedRealms.Repositories.Powers.Powers;
using ExpressedRealms.Server.EndPoints.PowerEndpoints.Responses.Options;
using ExpressedRealms.Server.EndPoints.PowerEndpoints.Responses.PowerList;
using ExpressedRealms.Server.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.PowerEndpoints;

internal static class PowerEndpoints
{
    internal static void AddPowerEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powers")
            .AddFluentValidationAutoValidation()
            .WithTags("Powers")
            .WithOpenApi();
        
        endpointGroup
            .MapGet(
                "/{expressionId}",
                async (
                    int expressionId,
                    IPowerRepository powerRepository) =>
                {
                    var powers = await powerRepository.GetPowersAsync(expressionId);
                    
                    return TypedResults.Ok(powers.Value.Select(x => new PowerInformationResponse()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Category = x.Category.Select(x => new DetailedInformation(x)).ToList(),
                        Description = x.Description,
                        GameMechanicEffect = x.GameMechanicEffect,
                        Limitation = x.Limitation,
                        PowerDuration = new DetailedInformation(x.PowerDuration),
                        AreaOfEffect = new DetailedInformation(x.AreaOfEffect),
                        PowerLevel = new DetailedInformation(x.PowerLevel),
                        PowerActivationType = new DetailedInformation(x.PowerActivationType),
                        Other = x.Other
                    }));
                }
            )
            .WithSummary("Returns the list of powers for a given expression")
            .WithDescription(" of powers for a given expression")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "/options",
                async (IPowerRepository powerRepository) =>
                {
                    var options = await powerRepository.GetPowerOptionsAsync();

                    return TypedResults.Ok(new PowerOptionsResponse()
                    {
                        Category = options.Value.Category.Select(x => new DetailedEditInformation(x)).ToList(),
                        PowerDuration =
                            options.Value.PowerDuration.Select(x => new DetailedEditInformation(x)).ToList(),
                        PowerLevel = options.Value.PowerLevel.Select(x => new DetailedEditInformation(x)).ToList(),
                        AreaOfEffect = options.Value.AreaOfEffect.Select(x => new DetailedEditInformation(x)).ToList(),
                        PowerActivationType = options.Value.PowerActivationType
                            .Select(x => new DetailedEditInformation(x)).ToList(),
                    });
                }
            )
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Returns available options for powers")
            .WithDescription("This endpoint retrieves the available options for creating or editing powers.");

        

        }
    }