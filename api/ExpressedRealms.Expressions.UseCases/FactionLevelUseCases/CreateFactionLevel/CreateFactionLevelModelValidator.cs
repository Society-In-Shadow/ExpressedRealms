using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.FactionLevelUseCases.CreateFactionLevel;

[UsedImplicitly]
internal sealed class CreateFactionLevelModelValidator : AbstractValidator<CreateFactionLevelModel>
{
    public CreateFactionLevelModelValidator()
    {
        RuleFor(x => x.FactionId).NotEmpty().WithMessage("Faction Id is required.");

        RuleFor(x => x.KnowledgeId).NotEmpty().WithMessage("Knowledge Id is required.");

        RuleFor(x => x.Specialization)
            .NotEmpty()
            .WithMessage("Specialization is required.")
            .MaximumLength(250)
            .WithMessage("Specialization cannot exceed 250 characters.");
    }
}
