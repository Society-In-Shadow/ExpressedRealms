using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.FinalizeCharacterCreate;

[UsedImplicitly]
public sealed class FinalizeCharacterCreateModelValidator
    : AbstractValidator<FinalizeCharacterCreateModel>
{
    public FinalizeCharacterCreateModelValidator(ICharacterRepository repository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await repository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}
