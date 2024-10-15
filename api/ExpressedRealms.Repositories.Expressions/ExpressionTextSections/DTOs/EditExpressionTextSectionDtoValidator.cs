using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using FluentValidation;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class EditExpressionTextSectionDtoValidator : AbstractValidator<EditExpressionDto>
{
    public EditExpressionTextSectionDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        RuleFor(x => x.ShortDescription).MaximumLength(125).NotEmpty();
        RuleFor(x => x.NavMenuImage).NotEmpty();
        RuleFor(x => x.PublishStatus).IsInEnum().NotEmpty();
    }
}
