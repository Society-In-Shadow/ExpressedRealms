using FluentValidation;

namespace ExpressedRealms.Repositories.Characters.DTOs;

internal sealed class EditCharacterDtoValidator : AbstractValidator<EditCharacterDto>
{
    public EditCharacterDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
    }
}
