using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.Characters.CopyCharacter;

[UsedImplicitly]
internal sealed class CopyCharacterModelValidator : AbstractValidator<CopyCharacterModel>
{
    public CopyCharacterModelValidator(ICharacterRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (x, y) => await repository.CharacterExistsAsync(x))
            .WithMessage("Character does not exist.");

        RuleFor(x => x.CharacterName).NotEmpty().MaximumLength(150);
    }
}
