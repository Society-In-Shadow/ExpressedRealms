using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathSorting;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.UpdatePowerPathSorting;

public static class UpdatePowerPathSortingEndpoint
{
    public static async Task<IResult> Execute(
        int expressionId,
        PowerPathOrderUpdateRequest request,
        IPowerPathRepository powerRepository
    )
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
}
