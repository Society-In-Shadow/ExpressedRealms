using ExpressedRealms.Powers.Repository.Powers;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerEndpoints.GetPowerOptions;

public static class GetPowerOptionsEndpoint
{
    public static async Task<IResult> Execute(IPowerRepository powerRepository)
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
                    .Value.PowerActivationType.Select(x => new DetailedEditInformation(x))
                    .ToList(),
            }
        );
    }
}
