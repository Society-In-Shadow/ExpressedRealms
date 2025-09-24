using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Admin.UseCases.UpdateCharacterXp;

[UsedImplicitly]
internal sealed class UpdateCharacterXpModelValidator : AbstractValidator<UpdateCharacterXpModel>
{
    public UpdateCharacterXpModelValidator(ICharacterRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.CharacterExistsAsync(x))
            .WithMessage("Character does not exist.");

        RuleFor(x => x.Xp)
            .NotEmpty()
            .WithMessage("Name is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Xp must be greater than or equal to 0.");
    }
}
