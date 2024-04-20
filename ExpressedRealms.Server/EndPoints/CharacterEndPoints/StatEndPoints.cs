using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;
using ExpressedRealms.Server.EndPoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints;

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
                "{id}",
                [Authorize]
                async Task<Results<NotFound, Ok<List<StatInfo>>>> (int id, ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var stats = await dbContext
                        .StateTypes
                        .Select(x => new StatInfo()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            StatLevel = 1,
                            StatLevels = x.StatDescriptionMappings.Select(x => new StatDetails()
                            {
                                Level = x.StatLevel.Id,
                                Bonus = x.StatLevel.Bonus,
                                XP = x.StatLevel.XPCost,
                                Description = x.ReasonableExpectation
                            }).ToList()
                        })
                        .ToListAsync();

                    var character = await dbContext.Characters.FirstOrDefaultAsync(x => x.Id == id);

                    if (character is null)
                        return TypedResults.NotFound();

                    stats.First(x => x.Id == 1).StatLevel = character.AgilityId;
                    stats.First(x => x.Id == 2).StatLevel = character.ConstitutionId;
                    stats.First(x => x.Id == 3).StatLevel = character.DexterityId;
                    stats.First(x => x.Id == 4).StatLevel = character.StrengthId;
                    stats.First(x => x.Id == 5).StatLevel = character.IntelligenceId;
                    stats.First(x => x.Id == 6).StatLevel = character.WillpowerId;
                    
                    return TypedResults.Ok(stats);
                }
            )
            .RequireAuthorization();
    }
}
