using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.Requests;

public class CreateCharacterRequestValidator : AbstractValidator<CreateCharacterRequest>
{
    public CreateCharacterRequestValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.ExpressionId).NotEmpty();
        RuleFor(x => x.FactionId).NotEmpty();

        RuleSet(
            "Async Checks",
            () =>
            {
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
                    .WithName(nameof(CreateCharacterRequest.FactionId))
                    .WithMessage("This is not a valid Faction Id");
            }
        );
    }
}
