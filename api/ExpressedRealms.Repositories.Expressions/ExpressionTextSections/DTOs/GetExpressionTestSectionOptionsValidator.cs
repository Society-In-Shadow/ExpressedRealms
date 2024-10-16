using ExpressedRealms.DB;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class GetExpressionTestSectionOptionsValidator
    : AbstractValidator<GetExpressionTestSectionOptionsDto>
{
    public GetExpressionTestSectionOptionsValidator(ExpressedRealmsDbContext dbContext)
    {
        RuleFor(x => x.SectionId)
            .MustAsync(
                async (sectionId, cancellationToken) =>
                {
                    return await dbContext.ExpressionSections.AnyAsync(
                        x => x.Id == sectionId,
                        cancellationToken
                    );
                }
            )
            .When(x => x.SectionId is not null)
            .WithMessage("This is not a valid Section");
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
    }
}
