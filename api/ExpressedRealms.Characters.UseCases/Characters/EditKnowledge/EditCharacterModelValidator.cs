using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ProgressionPaths;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.EditKnowledge;

internal sealed class EditCharacterModelValidator : AbstractValidator<EditCharacterModel>
{
    public EditCharacterModelValidator(
        IProgressionPathRepository progressionPathRepository, 
        ICharacterRepository characterRepository,
        IExpressionTextSectionRepository sectionRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null);
        
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        
        RuleFor(x => x)
            .MustAsync(async (x, y) =>
            {
                var character = await characterRepository.FindCharacterAsync(x.Id);
                return await sectionRepository.FindFactionForExpression(character!.ExpressionId, x.FactionId!.Value) is not null;
            })
            .WithName(nameof(EditCharacterModel.FactionId))
            .WithMessage("The Faction does not exist for this expression.")
            .When(x => x.FactionId.HasValue);
        
        RuleFor(x => x.PrimaryProgressionId)
            .MustAsync(
                async (x, y) => await progressionPathRepository.ProgressionPathExists(x!.Value)
            )
            .When(x => x.PrimaryProgressionId is not null);
        
        RuleFor(x => x.SecondaryProgressionId)
            .MustAsync(
                async (x, y) => await progressionPathRepository.ProgressionPathExists(x!.Value)
            )
            .When(x => x.SecondaryProgressionId is not null);
    }
}
