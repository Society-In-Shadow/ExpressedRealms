using ExpressedRealms.Characters.Repository;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

[UsedImplicitly]
internal sealed class GetKnowledgesForCharacterModelValidator
    : AbstractValidator<GetKnowledgesForCharacterModel>
{
    public GetKnowledgesForCharacterModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}
