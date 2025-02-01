using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters.Skills.DTOs;

public class EditCharacterSkillMappingDtoValidator : AbstractValidator<EditCharacterSkillMappingDto>
{
    public EditCharacterSkillMappingDtoValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    return await dbContext.Characters.AnyAsync(x => x.Id == dto, cancellationToken);
                }
            )
            .WithMessage("Character does not exist.");

        RuleFor(x => x.SkillTypeId)
            .NotEmpty()
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    return await dbContext.SkillTypes.AnyAsync(x => x.Id == dto, cancellationToken);
                }
            )
            .WithMessage("Skill Type does not exist.");

        RuleFor(x => x.SkillLevelId)
            .NotEmpty()
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    return await dbContext.SkillLevels.AnyAsync(
                        x => x.Id == dto,
                        cancellationToken
                    );
                }
            )
            .WithMessage("Skill Level does not exist.");

        RuleFor(x => x)
            .NotEmpty()
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    var currentSpentXp = await dbContext
                        .CharacterSkillsMappings.Where(x =>
                            x.CharacterId == dto.CharacterId && x.SkillTypeId != dto.SkillTypeId
                        )
                        .SumAsync(x => x.SkillLevel.XP, cancellationToken);

                    var newSkillLevelXp = await dbContext.SkillLevels.FirstAsync(
                        x => x.Id == dto.SkillLevelId,
                        cancellationToken
                    );

                    return currentSpentXp + newSkillLevelXp.XP < 28;
                }
            )
            .WithMessage("Not enough XP to level up.");
    }
}
