using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;

[UsedImplicitly]
internal sealed class CreateFactionModelValidator : AbstractValidator<CreateFactionModel>
{
    public CreateFactionModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name cannot exceed 250 characters.");

        RuleFor(x => x.Background)
            .NotEmpty()
            .WithMessage("Background is required.")
            .MaximumLength(20000)
            .WithMessage("Background cannot exceed 20000 characters.");

        RuleFor(x => x.ExpressionId).NotEmpty().WithMessage("Expression Id is required.");
    }
}
