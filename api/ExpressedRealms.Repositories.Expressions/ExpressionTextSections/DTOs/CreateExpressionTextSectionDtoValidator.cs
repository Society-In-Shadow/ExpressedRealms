using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Expressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public class CreateExpressionTextSectionDtoValidator : AbstractValidator<CreateExpressionTextSectionDto>
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
                        x =>
                             x.Id == sectionTypeId,
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
    }
}
