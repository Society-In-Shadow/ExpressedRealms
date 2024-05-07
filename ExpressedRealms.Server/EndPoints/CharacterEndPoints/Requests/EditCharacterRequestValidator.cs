using ExpressedRealms.DB;
using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;

public class EditCharacterRequestValidator : AbstractValidator<EditCharacterRequest>
{
    public EditCharacterRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.FactionId).NotEmpty();
    }
}
