using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerEndpoints.GetPower;

public static class GetPowerEndpoint
{
    public static async Task<Results<NotFound, Ok<EditPowerInformationResponse>>> Execute(
        int powerId,
        IPowerRepository powerRepository
    )
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
                StatModifierGroup = powers.Value.StatModifierGroup,
            }
        );
    }
}
