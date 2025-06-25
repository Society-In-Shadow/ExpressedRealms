using ExpressedRealms.Characters.API.CharacterEndPoints.StatDTOs;
using ExpressedRealms.Characters.Repository.Stats.Enums;
using ExpressedRealms.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Characters.API.StatEndPoints;

internal static class StatEndPoints
{
    internal static void AddStatEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("stats")
            .AddFluentValidationAutoValidation()
            .WithTags("Stats")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "{statTypeId}",
                [Authorize]
                async Task<Results<NotFound, Ok<List<StatDetails>>>> (
                    StatType statTypeId,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var stats = await dbContext
                        .StatDescriptionMappings.Where(x => x.StatTypeId == (byte)statTypeId)
                        .OrderBy(x => x.StatLevel.Id)
                        .Select(x => new StatDetails()
                        {
                            Level = x.StatLevel.Id,
                            Bonus = x.StatLevel.Bonus,
                            XP = x.StatLevel.XPCost,
                            TotalXP = x.StatLevel.TotalXPCost,
                            Description = x.ReasonableExpectation,
                        })
                        .ToListAsync();

                    return TypedResults.Ok(stats);
                }
            )
            .WithSummary("Returns all levels for the given stat")
            .WithDescription(
                "This will return the levels for the given stat id.  It will be in the range of 1-7."
            )
            .RequireAuthorization();
    }
}
