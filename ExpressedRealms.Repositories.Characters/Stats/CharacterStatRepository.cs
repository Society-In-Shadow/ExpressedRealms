using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Characters.Stats.DTOs;
using ExpressedRealms.Repositories.Characters.Stats.Enums;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters.Stats;

internal sealed class CharacterStatRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    GetDetailedStatInfoDtoValidator detailedStatValidator,
    CancellationToken cancellationToken
) : ICharacterStatRepository
{
    public async Task<Result<SingleStatInfo>> GetDetailedStatInfo(GetDetailedStatInfoDto dto)
    {
        var result = await detailedStatValidator.ValidateAsync(dto);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var character = await context
            .Characters.Where(x => x.Id == dto.CharacterId && x.Player.UserId == userContext.CurrentUserId())
            .Select(x => new
            {
                AgilityId = x.AgilityId,
                ConstitutionId = x.ConstitutionId,
                DexterityId = x.DexterityId,
                StrengthId = x.StrengthId,
                IntelligenceId = x.IntelligenceId,
                WillpowerId = x.WillpowerId,
                AvailableXP = x.StatExperiencePoints
                    - (
                        x.AgilityStatLevel.TotalXPCost
                        + x.ConstitutionStatLevel.TotalXPCost
                        + x.DexterityStatLevel.TotalXPCost
                        + x.StrengthStatLevel.TotalXPCost
                        + x.IntelligenceStatLevel.TotalXPCost
                        + x.WillpowerStatLevel.TotalXPCost
                    )
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        var statLevelId = dto.StatTypeId switch
        {
            StatType.Agility => character.AgilityId,
            StatType.Constitution => character.ConstitutionId,
            StatType.Dexterity => character.DexterityId,
            StatType.Strength => character.StrengthId,
            StatType.Intelligence => character.IntelligenceId,
            StatType.Willpower => character.WillpowerId,
        };

        var statInfo = await context
            .StateTypes.Where(x => x.Id == (byte)dto.StatTypeId)
            .Select(x => new SingleStatInfo()
            {
                Id = (StatType)x.Id,
                Name = x.Name,
                Description = x.Description,
                StatLevel = statLevelId,
                AvailableXP = character.AvailableXP,
                StatLevelInfo = x
                    .StatDescriptionMappings.Where(y => y.StatLevelId == statLevelId)
                    .Select(y => new StatDetails()
                    {
                        Level = y.StatLevelId,
                        XP = y.StatLevel.XPCost,
                        Bonus = y.StatLevel.Bonus,
                        Description = y.ReasonableExpectation,
                        TotalXP = y.StatLevel.TotalXPCost
                    })
                    .First()
            })
            .FirstAsync(cancellationToken);

        return Result.Ok(statInfo);
    }
}