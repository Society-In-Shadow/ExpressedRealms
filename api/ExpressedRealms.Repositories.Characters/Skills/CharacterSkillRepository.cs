using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.Repositories.Characters.Skills.DTOs;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters.Skills;

internal sealed class CharacterSkillRepository(
    ExpressedRealmsDbContext context,
    EditCharacterSkillMappingDtoValidator editCharacterSkillMappingDtoValidator,
    CancellationToken cancellationToken
) : ICharacterSkillRepository
{
    public async Task AddDefaultSkills(int characterId)
    {
        var availableSkills = await context.SkillTypes.AsNoTracking().ToListAsync();

        var characterSkills = availableSkills.Select(x => new CharacterSkillsMapping()
        {
            CharacterId = characterId,
            SkillTypeId = x.Id,
            SkillLevelId = 1, // Untrained
        });

        context.CharacterSkillsMappings.AddRange(characterSkills);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SkillDto>> GetCharacterSkills(int characterId)
    {
        return await context
            .CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new SkillDto()
            {
                SkillTypeId = x.SkillTypeId,
                Description = x.SkillType.Description,
                SkillSubTypeId = x.SkillType.SkillSubTypeId,
                Name = x.SkillType.Name,
                LevelId = x.SkillLevelId,
                LevelName = x.SkillLevel.Name,
                XP = x.SkillLevel.XP,
                LevelDescription = x
                    .SkillType.CharacterLevelDescriptions.First(y =>
                        y.SkillLevelId == x.SkillLevelId
                    )
                    .Description,
            })
            .OrderBy(x => x.SkillTypeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SkillLevelOptionsDto>> GetSkillLevelValuesForSkillTypeId(int skillTypeId)
    {
        var descriptions = await context
            .SkillLevelDescriptionMappings.AsNoTracking()
            .Where(x => x.SkillTypeId == skillTypeId)
            .Select(x => new SkillLevelOptionsDto()
            {
                SkillTypeId = x.SkillTypeId,
                Name = x.SkillLevel.Name,
                Description = x.Description,
                LevelId = x.SkillLevelId,
                ExperienceCost = x.SkillLevel.XP,
            })
            .ToListAsync(cancellationToken);

        var benefits = await context
            .SkillLevelBenefits.AsNoTracking()
            .Where(x => x.SkillTypeId == skillTypeId)
            .Select(x => new BenefitDto()
            {
                LevelId = x.SkillLevelId,
                Name = x.ModifierType.Name,
                Description = x.ModifierType.Description,
                Modifier = x.Modifier,
            })
            .ToListAsync(cancellationToken);

        foreach (var description in descriptions)
        {
            description.Benefits = benefits.Where(x => x.LevelId == description.LevelId).ToList();
        }

        return descriptions;
    }

    public async Task<Result> UpdateSkillLevel(EditCharacterSkillMappingDto dto)
    {
        var result = await editCharacterSkillMappingDtoValidator.ValidateAsync(
            dto,
            cancellationToken
        );
        if (!result.IsValid)
            return Result.Fail(new FluentValidationFailure(result.ToDictionary()));

        var characterSkill = await context.CharacterSkillsMappings.FirstOrDefaultAsync(
            x => x.CharacterId == dto.CharacterId && x.SkillTypeId == dto.SkillTypeId,
            cancellationToken
        );

        if (characterSkill is null)
            return Result.Fail(new NotFoundFailure("Character Skill Mapping"));

        characterSkill.SkillLevelId = dto.SkillLevelId;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
