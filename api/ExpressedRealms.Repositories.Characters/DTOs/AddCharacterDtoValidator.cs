using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Characters.Enums;
using ExpressedRealms.Repositories.Expressions.Expressions;
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
                        x =>
                            x.Id == expressionId
                            && x.PublishStatusId == (int)PublishTypes.Published
                            && x.ExpressionTypeId == 1,
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
                            && x.Expression.PublishStatusId == (int)PublishTypes.Published
                            && x.Expression.ExpressionTypeId == 1
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
