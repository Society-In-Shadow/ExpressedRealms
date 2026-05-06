using ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using Microsoft.AspNetCore.Http;
using DetailedInformation = ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath.DetailedInformation;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.GetPowerPathsForExpression;

public static class GetPowerPathsForExpressionEndpoint
{
    public static async Task<IResult> Execute(
        int expressionId,
        IPowerPathRepository powerRepository
    )
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
                        PowerActivationType = new DetailedInformation(y.PowerActivationType),
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
}
