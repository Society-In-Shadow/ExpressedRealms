using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath;

public static class GetPowersForPowerPathEndpoint
{
    public static async Task<IResult> Execute(int id, IPowerRepository powerRepository)
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
}