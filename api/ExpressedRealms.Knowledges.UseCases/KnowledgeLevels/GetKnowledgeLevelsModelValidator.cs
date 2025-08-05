using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public class GetKnowledgeLevelsModelValidator : AbstractValidator<GetKnowledgeLevelsModel>
{
    public GetKnowledgeLevelsModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}