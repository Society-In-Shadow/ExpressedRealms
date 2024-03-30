using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.DTOs;

public class CreateCharacterDTOValidator : AbstractValidator<CreateCharacterDTO>
{
    public CreateCharacterDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.ExpressionId).NotEmpty().InclusiveBetween(1, 6);
    }
}
