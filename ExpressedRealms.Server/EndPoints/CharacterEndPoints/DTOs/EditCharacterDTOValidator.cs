using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;

public class EditCharacterDTOValidator : AbstractValidator<EditCharacterDTO>
{
    public EditCharacterDTOValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}
