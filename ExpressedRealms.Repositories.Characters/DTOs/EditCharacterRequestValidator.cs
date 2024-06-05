using FluentValidation;

namespace ExpressedRealms.Repositories.Characters.DTOs;

public class EditCharacterRequestValidator : AbstractValidator<EditCharacterDTO>
{
    public EditCharacterRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.FactionId).NotEmpty();
    }
}
