using FluentValidation;

namespace ExpressedRealms.Characters.Repository.DTOs;

internal sealed class AddCharacterDtoValidator : AbstractValidator<AddCharacterDto>
{
    public AddCharacterDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.ExpressionId).NotEmpty();
    }
}
