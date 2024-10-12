using FluentValidation;

namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public class CreateExpressionDtoValidator : AbstractValidator<CreateExpressionDto>
{
    public CreateExpressionDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.NavMenuImage).NotEmpty();
    }
}
