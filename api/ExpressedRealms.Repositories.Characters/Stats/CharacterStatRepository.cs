using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
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
    EditStatDtoValidator editStatValidator,
    CancellationToken cancellationToken
) : ICharacterStatRepository
{
    public async Task<Result<SingleStatInfo>> GetDetailedStatInfo(GetDetailedStatInfoDto dto)
    {
        var result = await detailedStatValidator.ValidateAsync(dto);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var character = await context
            .Characters.Where(x =>
                x.Id == dto.CharacterId && x.Player.UserId == userContext.CurrentUserId()
            )
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
                    ),
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
                        TotalXP = y.StatLevel.TotalXPCost,
                    })
                    .First(),
            })
            .FirstAsync(cancellationToken);

        return Result.Ok(statInfo);
    }

    public async Task<Result> UpdateCharacterStat(EditStatDto dto)
    {
        var result = await editStatValidator.ValidateAsync(dto);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var character = await context
            .Characters.Where(x =>
                x.Id == dto.CharacterId && x.Player.UserId == userContext.CurrentUserId()
            )
            .Include(x => x.AgilityStatLevel)
            .Include(x => x.StrengthStatLevel)
            .Include(x => x.ConstitutionStatLevel)
            .Include(x => x.DexterityStatLevel)
            .Include(x => x.IntelligenceStatLevel)
            .Include(x => x.WillpowerStatLevel)
            .FirstOrDefaultAsync(cancellationToken);

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        var xpCheck = await EditStatXpCheck(dto, character);
        if (xpCheck.IsFailed)
            return xpCheck;

        switch (dto.StatTypeId)
        {
            case StatType.Agility:
                character.AgilityId = dto.LevelTypeId;
                break;
            case StatType.Constitution:
                character.ConstitutionId = dto.LevelTypeId;
                break;
            case StatType.Dexterity:
                character.DexterityId = dto.LevelTypeId;
                break;
            case StatType.Strength:
                character.StrengthId = dto.LevelTypeId;
                break;
            case StatType.Intelligence:
                character.IntelligenceId = dto.LevelTypeId;
                break;
            case StatType.Willpower:
                character.WillpowerId = dto.LevelTypeId;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private async Task<Result> EditStatXpCheck(EditStatDto dto, Character character)
    {
        var availableXp = await context
            .Characters.Where(x => x.Id == dto.CharacterId)
            .Select(x =>
                x.StatExperiencePoints
                - (
                    x.AgilityStatLevel.TotalXPCost
                    + x.ConstitutionStatLevel.TotalXPCost
                    + x.DexterityStatLevel.TotalXPCost
                    + x.StrengthStatLevel.TotalXPCost
                    + x.IntelligenceStatLevel.TotalXPCost
                    + x.WillpowerStatLevel.TotalXPCost
                )
            )
            .FirstOrDefaultAsync(cancellationToken);

        var oldTotalXpCost = dto.StatTypeId switch
        {
            StatType.Agility => character.AgilityStatLevel.TotalXPCost,
            StatType.Strength => character.StrengthStatLevel.TotalXPCost,
            StatType.Constitution => character.ConstitutionStatLevel.TotalXPCost,
            StatType.Dexterity => character.DexterityStatLevel.TotalXPCost,
            StatType.Intelligence => character.IntelligenceStatLevel.TotalXPCost,
            StatType.Willpower => character.WillpowerStatLevel.TotalXPCost,
            _ => throw new ArgumentOutOfRangeException(),
        };

        var newTotalXpCost = await context
            .StatLevels.Where(x => x.Id == dto.LevelTypeId)
            .Select(x => x.TotalXPCost)
            .FirstAsync(cancellationToken);

        if (availableXp < newTotalXpCost - oldTotalXpCost)
        {
            return Result.Fail(
                new NotEnoughXPFailure(availableXp, newTotalXpCost - oldTotalXpCost)
            );
        }

        return Result.Ok();
    }

    public async Task<Result<List<SmallStatInfo>>> GetAllStats(int characterId)
    {
        var character = await context
            .Characters.Include(x => x.AgilityStatLevel)
            .Include(x => x.ConstitutionStatLevel)
            .Include(x => x.DexterityStatLevel)
            .Include(x => x.StrengthStatLevel)
            .Include(x => x.IntelligenceStatLevel)
            .Include(x => x.WillpowerStatLevel)
            .FirstOrDefaultAsync(
                x => x.Id == characterId && x.Player.UserId == userContext.CurrentUserId(),
                cancellationToken
            );

        if (character is null)
            return Result.Fail(new NotFoundFailure("Character"));

        var statTypes = await context.StateTypes.ToListAsync(cancellationToken);

        var characterStats = new List<SmallStatInfo>()
        {
            new()
            {
                StatTypeId = StatType.Agility,
                Bonus = character.AgilityStatLevel.Bonus,
                Level = character.AgilityStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Agility).ShortName,
            },
            new()
            {
                StatTypeId = StatType.Constitution,
                Bonus = character.ConstitutionStatLevel.Bonus,
                Level = character.ConstitutionStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Constitution).ShortName,
            },
            new()
            {
                StatTypeId = StatType.Dexterity,
                Bonus = character.DexterityStatLevel.Bonus,
                Level = character.DexterityStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Dexterity).ShortName,
            },
            new()
            {
                StatTypeId = StatType.Strength,
                Bonus = character.StrengthStatLevel.Bonus,
                Level = character.StrengthStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Strength).ShortName,
            },
            new()
            {
                StatTypeId = StatType.Intelligence,
                Bonus = character.IntelligenceStatLevel.Bonus,
                Level = character.IntelligenceStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Intelligence).ShortName,
            },
            new()
            {
                StatTypeId = StatType.Willpower,
                Bonus = character.WillpowerStatLevel.Bonus,
                Level = character.WillpowerStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Willpower).ShortName,
            },
        };

        return Result.Ok(characterStats);
    }
}
