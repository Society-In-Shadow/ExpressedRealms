using ExpressedRealms.DB;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.Helpers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;

public class CreateExpressionTextSectionDtoValidator
    : AbstractValidator<CreateExpressionTextSectionDto>
{
    public CreateExpressionTextSectionDtoValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.SectionTypeId)
            .MustAsync(
                async (sectionTypeId, cancellationToken) =>
                {
                    return await dbContext.ExpressionSectionTypes.AnyAsync(
                        x => x.Id == sectionTypeId,
                        cancellationToken
                    );
                }
            )
            .WithMessage("This is not a valid Section Type");
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
                async (expressionSection, cancellationToken) =>
                {
                    var expressionSections = await dbContext
                        .ExpressionSections.Where(x =>
                            x.ExpressionId == expressionSection.ExpressionId
                        )
                        .ToListAsync();

                    var validParentIds = RecursiveFunctions.GetValidParentIds(
                        expressionSections,
                        null,
                        0
                    );
                    return validParentIds.Contains(expressionSection.ParentId.Value);
                }
            )
            .When(x => x.ParentId != null)
            .WithMessage("This is not a valid Parent Id");
    }
}
