using FluentValidation;

namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public class EditExpressionDtoValidator : AbstractValidator<EditExpressionDto>
{
    public EditExpressionDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        RuleFor(x => x.ShortDescription).MaximumLength(125).NotEmpty();
        RuleFor(x => x.NavMenuImage).NotEmpty();
        RuleFor(x => x.PublishStatus).IsInEnum().NotEmpty();
    }
}
