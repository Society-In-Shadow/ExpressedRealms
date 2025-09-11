using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Get;

[UsedImplicitly]
internal sealed class GetAssignedBlessingsModelValidator : AbstractValidator<GetAssignedBlessingsModel>
{
    public GetAssignedBlessingsModelValidator(ICharacterRepository repository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await repository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}