using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerSorting;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerEndpoints.UpdatePowerPathSorting;

public static class UpdatePowerPathPowerSortingEndpoint
{
    public static async Task<IResult> Execute(
        int powerPathId,
        PowerOrderUpdateRequest request,
        IPowerRepository powerRepository
    )
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
}
