using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.DTOs;

public class CreateCharacterDTOValidator : AbstractValidator<CreateCharacterDTO>
{
    public CreateCharacterDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}
