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
                "{statTypeId}",
                [Authorize]
                async Task<Results<NotFound, Ok<List<StatDetails>>>> (
                    int statTypeId,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var stats = await dbContext
                        .StatDescriptionMappings.Where(x => x.StatTypeId == statTypeId)
                        .Select(x => new StatDetails()
                        {
                            Level = x.StatLevel.Id,
                            Bonus = x.StatLevel.Bonus,
                            XP = x.StatLevel.XPCost,
                            Description = x.ReasonableExpectation
                        })
                        .ToListAsync();

                    return TypedResults.Ok(stats);
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{id}/smallStats",
                [Authorize]
                async Task<Results<NotFound, Ok<List<SmallStatInfo>>>> (
                    int id,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var character = await dbContext
                        .Characters.Include(x => x.AgilityStatLevel)
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
                        new()
                        {
                            StatTypeId = 1,
                            Bonus = character.AgilityStatLevel.Bonus,
                            Level = character.AgilityStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 1).ShortName
                        },
                        new()
                        {
                            StatTypeId = 2,
                            Bonus = character.ConstitutionStatLevel.Bonus,
                            Level = character.ConstitutionStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 2).ShortName
                        },
                        new()
                        {
                            StatTypeId = 3,
                            Bonus = character.DexterityStatLevel.Bonus,
                            Level = character.DexterityStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 3).ShortName
                        },
                        new()
                        {
                            StatTypeId = 4,
                            Bonus = character.StrengthStatLevel.Bonus,
                            Level = character.StrengthStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 4).ShortName
                        },
                        new()
                        {
                            StatTypeId = 5,
                            Bonus = character.IntelligenceStatLevel.Bonus,
                            Level = character.IntelligenceStatLevel.Id,
                            ShortName = statTypes.First(x => x.Id == 5).ShortName
                        },
                        new()
                        {
                            StatTypeId = 6,
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
