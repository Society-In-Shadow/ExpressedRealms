using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Characters.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters.DTOs;

internal sealed class AddCharacterDtoValidator : AbstractValidator<AddCharacterDto>
{
    public AddCharacterDtoValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.ExpressionId).NotEmpty();
        RuleFor(x => x.ExpressionId)
            .MustAsync(
                async (expressionId, cancellationToken) =>
                {
                    return await dbContext.Expressions.AnyAsync(
                        x => x.Id == expressionId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Expression Id");
        RuleFor(x => x)
            .MustAsync(
                async (dto, cancellationToken) =>
                {
                    return await dbContext.ExpressionSections.AnyAsync(
                        x =>
                            x.ExpressionId == dto.ExpressionId
                            && x.SectionTypeId == (int)ExpressionSectionType.FactionType
                            && x.Id == dto.FactionId,
                        cancellationToken
                    );
                }
            )
            .When(x => x.FactionId is not null)
            .WithName(nameof(AddCharacterDto.FactionId))
            .WithMessage("This is not a valid Faction Id");
    }
}
