using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

[UsedImplicitly]
internal sealed class GetCharacterExperienceBreakdownModelValidator
    : AbstractValidator<GetCharacterExperienceBreakdownModel>
{
    public GetCharacterExperienceBreakdownModelValidator(
        ICharacterKnowledgeRepository mappingRepository,
        ICharacterRepository characterRepository
    )
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character Id does not exist.");
    }
}
