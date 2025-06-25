using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Enums;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Statistics;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Proficiencies;

internal sealed class ProficiencyRepository (ExpressedRealmsDbContext context, 
    IUserContext userContext,
    CancellationToken cancellationToken) : IProficiencyRepository
{
    public async Task<Result<List<ProficiencyDto>>> GetBasicProficiencies(int characterId)
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
        {
            return Result.Fail(new NotFoundFailure("Character"));
        }

        var statLevels = new List<StatLevel>()
        {
            character.AgilityStatLevel,
            character.ConstitutionStatLevel,
            character.DexterityStatLevel,
            character.StrengthStatLevel,
            character.IntelligenceStatLevel,
            character.WillpowerStatLevel,
        };

        var skills = await context.CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.SkillTypeId,
                x.SkillLevel.Level
            })
            .ToListAsync();


        int CalculateBasicValue(StatLevels firstStat, StatLevels secondStat, SkillTypes skillType)
        {
            return statLevels.First(x => x.Id == (byte)firstStat).Bonus + 
                   statLevels.First(x => x.Id == (byte)secondStat).Bonus +
                   skills.First(x => x.SkillTypeId == (byte)skillType).Level;
        }

        return new List<ProficiencyDto>()
        {
            new ProficiencyDto()
            {
                OffensiveName = "Strike",
                OffensiveValue = CalculateBasicValue(StatLevels.Dexterity, StatLevels.Strength, SkillTypes.HandToHandOffense),
                DefensiveName = "Dodge",
                DefensiveValue = CalculateBasicValue(StatLevels.Agility, StatLevels.Strength, SkillTypes.HandToHandDefense)
            },
            new ProficiencyDto()
            {
                OffensiveName = "Thrust",
                OffensiveValue = CalculateBasicValue(StatLevels.Agility, StatLevels.Dexterity, SkillTypes.MeleeOffense),
                DefensiveName = "Parry",
                DefensiveValue = CalculateBasicValue(StatLevels.Agility, StatLevels.Strength, SkillTypes.MeleeDefense)
            },
            new ProficiencyDto()
            {
                OffensiveName = "Throw",
                OffensiveValue = CalculateBasicValue(StatLevels.Dexterity, StatLevels.Intelligence, SkillTypes.ThrownWeapons),
                DefensiveName = "Evade",
                DefensiveValue = CalculateBasicValue(StatLevels.Agility, StatLevels.Intelligence, SkillTypes.Acrobatics)
            },
            new ProficiencyDto()
            {
                OffensiveName = "Shoot",
                OffensiveValue = CalculateBasicValue(StatLevels.Dexterity, StatLevels.Intelligence, SkillTypes.Marksmanship),
                DefensiveName = "Evade",
                DefensiveValue = CalculateBasicValue(StatLevels.Agility, StatLevels.Intelligence, SkillTypes.Acrobatics)
            },
            new ProficiencyDto()
            {
                OffensiveName = "Cast",
                OffensiveValue = CalculateBasicValue(StatLevels.Intelligence, StatLevels.Willpower, SkillTypes.Spellcasting),
                DefensiveName = "Ward",
                DefensiveValue = CalculateBasicValue(StatLevels.Constitution, StatLevels.Willpower, SkillTypes.Spellwarding)
            },
            new ProficiencyDto()
            {
                OffensiveName = "Project",
                OffensiveValue = CalculateBasicValue(StatLevels.Intelligence, StatLevels.Willpower, SkillTypes.Projection),
                DefensiveName = "Deflect",
                DefensiveValue = CalculateBasicValue(StatLevels.Constitution, StatLevels.Willpower, SkillTypes.Deflection)
            },
        };
    }
}