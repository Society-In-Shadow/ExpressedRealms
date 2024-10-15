using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using FluentValidation;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class CreateExpressionTextSectionDtoValidator : AbstractValidator<CreateExpressionDto>
{
    public CreateExpressionTextSectionDtoValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        RuleFor(x => x.ShortDescription).MaximumLength(125).NotEmpty();
        RuleFor(x => x.NavMenuImage).NotEmpty();
    }
}
