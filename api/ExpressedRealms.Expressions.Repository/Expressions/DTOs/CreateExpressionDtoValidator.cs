using FluentValidation;

namespace ExpressedRealms.Expressions.Repository.Expressions.DTOs;

public class CreateExpressionDtoValidator : AbstractValidator<CreateExpressionDto>
{
    public CreateExpressionDtoValidator()
    {
        RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
        RuleFor(x => x.ShortDescription).MaximumLength(125).NotEmpty();
        RuleFor(x => x.NavMenuImage).NotEmpty();
    }
}
