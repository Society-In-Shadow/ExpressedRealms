using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.EditFaction;

[UsedImplicitly]
internal sealed class EditFactionModelValidator : AbstractValidator<EditFactionModel>
{
    public EditFactionModelValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

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
    }
}
