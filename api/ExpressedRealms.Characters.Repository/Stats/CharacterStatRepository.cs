using ExpressedRealms.Characters.Repository.Stats.DTOs;
using ExpressedRealms.Characters.Repository.Stats.Enums;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Characters.XpTables;
using ExpressedRealms.DB.Models.Statistics.CharacterStatMappings;
using ExpressedRealms.Repositories.Shared;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using FluentValidationFailure = ExpressedRealms.Repositories.Shared.CommonFailureTypes.FluentValidationFailure;
using NotFoundFailure = ExpressedRealms.Repositories.Shared.CommonFailureTypes.NotFoundFailure;

namespace ExpressedRealms.Characters.Repository.Stats;

internal sealed class CharacterStatRepository(
    ExpressedRealmsDbContext context,
    IUserContext userContext,
    IXpRepository xpRepository,
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
        
        var query = await context
            .Characters.AsNoTracking()
            .WithUserAccessAsync(userContext, dto.CharacterId);
            
        var character = await query.Select(x => x.Id).FirstOrDefaultAsync();
        if (character == 0)
            return Result.Fail(new NotFoundFailure("Character"));
        
        var stats = await context.CharacterStatMappings.AsNoTracking()
            .Where(x => x.CharacterId == dto.CharacterId)
            .Select(x => new
            {
                x.Id,
                x.StatTypeId,
                x.StatLevelId,
                x.StatLevel.TotalXPCost
            })
            .ToListAsync(cancellationToken);
        
        var statMapping = stats.First(x => x.StatTypeId == (byte)dto.StatTypeId);

        var statInfo = await context
            .StateTypes.Where(x => x.Id == (byte)dto.StatTypeId)
            .Select(x => new SingleStatInfo()
            {
                Name = x.Name,
                Description = x.Description,
                StatLevelInfo = x
                    .StatDescriptionMappings.Where(y => y.StatLevelId == statMapping.StatLevelId)
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

        statInfo.Id = dto.StatTypeId;
        statInfo.StatLevel = statMapping.StatLevelId;
        statInfo.AvailableXP = StartingExperience.StartingStats - stats.Sum(x => x.TotalXPCost);

        return Result.Ok(statInfo);
    }

    public async Task<Result> UpdateCharacterStat(EditStatDto dto)
    {
        var result = await editStatValidator.ValidateAsync(dto);
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var query = await context
            .Characters.AsNoTracking()
            .WithUserAccessAsync(userContext, dto.CharacterId);
            
        var character = await query.Select(x => x.Id).FirstOrDefaultAsync();
        if (character == 0)
            return Result.Fail(new NotFoundFailure("Character"));

        var mapping = await context.CharacterStatMappings
            .Where(x => x.CharacterId == dto.CharacterId && x.StatTypeId == (byte)dto.StatTypeId)
            .FirstAsync();

        var xpCheck = await EditStatXpCheck(dto, mapping);
        if (xpCheck.IsFailed)
            return xpCheck;

        mapping.StatLevelId = dto.LevelTypeId;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private async Task<Result> EditStatXpCheck(EditStatDto dto, CharacterStatMapping statMapping)
    {
        var xpInfo = await xpRepository.GetAvailableXpForSection(
            statMapping.CharacterId,
            XpSectionTypes.Stats
        );
        
        var levels = new List<byte>() { statMapping.StatLevelId, dto.LevelTypeId };
        var types = await context
            .StatLevels.Where(x => levels.Contains(x.Id))
            .Select(x => new
            {
                x.Id,
                x.TotalXPCost
            })
            .ToListAsync(cancellationToken);

        var oldTotalXpCost = types.First(x => x.Id == statMapping.StatLevelId).TotalXPCost;
        var newTotalXpCost = types.First(x => x.Id == dto.LevelTypeId).TotalXPCost;
        var spentXp = xpInfo.SpentXp;

        spentXp -= oldTotalXpCost;

        if (spentXp + newTotalXpCost > xpInfo.AvailableXp)
        {
            return Result.Fail(
                new NotEnoughXPFailure(
                    xpInfo.AvailableXp - xpInfo.SpentXp,
                    newTotalXpCost - oldTotalXpCost
                )
            );
        }

        return Result.Ok();
    }

    public async Task<Result<List<SmallStatInfo>>> GetAllStats(int characterId)
    {
        var query = await context
            .Characters.AsNoTracking()
            .WithUserAccessAsync(userContext, characterId);

        var character = await query
            .Include(x => x.AgilityStatLevel)
            .Include(x => x.ConstitutionStatLevel)
            .Include(x => x.DexterityStatLevel)
            .Include(x => x.StrengthStatLevel)
            .Include(x => x.IntelligenceStatLevel)
            .Include(x => x.WillpowerStatLevel)
            .FirstOrDefaultAsync();

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
                Name = statTypes.First(x => x.Id == (byte)StatType.Agility).Name,
            },
            new()
            {
                StatTypeId = StatType.Constitution,
                Bonus = character.ConstitutionStatLevel.Bonus,
                Level = character.ConstitutionStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Constitution).ShortName,
                Name = statTypes.First(x => x.Id == (byte)StatType.Constitution).Name,
            },
            new()
            {
                StatTypeId = StatType.Dexterity,
                Bonus = character.DexterityStatLevel.Bonus,
                Level = character.DexterityStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Dexterity).ShortName,
                Name = statTypes.First(x => x.Id == (byte)StatType.Dexterity).Name,
            },
            new()
            {
                StatTypeId = StatType.Strength,
                Bonus = character.StrengthStatLevel.Bonus,
                Level = character.StrengthStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Strength).ShortName,
                Name = statTypes.First(x => x.Id == (byte)StatType.Strength).Name,
            },
            new()
            {
                StatTypeId = StatType.Intelligence,
                Bonus = character.IntelligenceStatLevel.Bonus,
                Level = character.IntelligenceStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Intelligence).ShortName,
                Name = statTypes.First(x => x.Id == (byte)StatType.Intelligence).Name,
            },
            new()
            {
                StatTypeId = StatType.Willpower,
                Bonus = character.WillpowerStatLevel.Bonus,
                Level = character.WillpowerStatLevel.Id,
                ShortName = statTypes.First(x => x.Id == (byte)StatType.Willpower).ShortName,
                Name = statTypes.First(x => x.Id == (byte)StatType.Willpower).Name,
            },
        };

        return Result.Ok(characterStats);
    }

    public async Task<int> GetExperienceSpentOnStatsForCharacter(int characterId)
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

        return character!.AgilityStatLevel.TotalXPCost
            + character.ConstitutionStatLevel.TotalXPCost
            + character.DexterityStatLevel.TotalXPCost
            + character.StrengthStatLevel.TotalXPCost
            + character.IntelligenceStatLevel.TotalXPCost
            + character.WillpowerStatLevel.TotalXPCost;
    }
}
