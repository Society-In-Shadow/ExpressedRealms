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
        
        endpointGroup
            .MapGet(
                "{id}/smallStats",
                [Authorize]
                async Task<Results<NotFound, Ok<List<SmallStatInfo>>>> (int id, ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var character = await dbContext.Characters
                        .Include(x => x.AgilityStatLevel)
                        .Include(x => x.ConstitutionStatLevel)
                        .Include(x => x.DexterityStatLevel)
                        .Include(x => x.StrengthStatLevel)
                        .Include(x => x.IntelligenceStatLevel)
                        .Include(x => x.WillpowerStatLevel)
                        .FirstOrDefaultAsync(x => x.Id == id);

                    var statTypes = await dbContext.StateTypes.ToListAsync();
                    if (character is null)
                        return TypedResults.NotFound();

                    var characterStats = new List<SmallStatInfo>()
                    {
                        new ()
                        {
                            Bonus = character.AgilityStatLevel.Bonus,
                            Level = character.AgilityStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 1).ShortName
                        },
                        new ()
                        {
                            Bonus = character.ConstitutionStatLevel.Bonus,
                            Level = character.ConstitutionStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 2).ShortName
                        },
                        new ()
                        {
                            Bonus = character.DexterityStatLevel.Bonus,
                            Level = character.DexterityStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 3).ShortName
                        },
                        new ()
                        {
                            Bonus = character.StrengthStatLevel.Bonus,
                            Level = character.StrengthStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 4).ShortName
                        },
                        new ()
                        {
                            Bonus = character.IntelligenceStatLevel.Bonus,
                            Level = character.IntelligenceStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 5).ShortName
                        },
                        new ()
                        {
                            Bonus = character.WillpowerStatLevel.Bonus,
                            Level = character.WillpowerStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 6).ShortName
                        }
                    };
                    
                    return TypedResults.Ok(characterStats);
                }
            )
            .RequireAuthorization();
    }
}
