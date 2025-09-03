using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.EditBlessingLevel;

[UsedImplicitly]
internal sealed class EditBlessingLevelModelValidator : AbstractValidator<EditBlessingLevelModel>
{
    public EditBlessingLevelModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("Level is required.")
            .MaximumLength(25)
            .WithMessage("Name must be between 1 and 25 characters.")
            .Matches(@"^\d+ pts?\.$")
            .WithMessage("Level must be in the format of '123 pts.' or '1 pt.'");

        RuleFor(x => x.LevelId)
            .NotEmpty()
            .WithMessage("Level Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessingLevel(x))
            .WithMessage("Level Id does not exist.");
        
        RuleFor(x => x)
            .MustAsync(
                async (x, y) => !await repository.HasDuplicateLevelName(x.BlessingId, x.Level)
            )
            .WithName(nameof(EditBlessingLevelModel.Level))
            .WithMessage("Blessing already has a level with this name.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.XpCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Xp Cost must be greater than or equal to 0.");

        RuleFor(x => x.XpGain)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Xp Gain must be greater than or equal to 0.");

        RuleFor(x => x.BlessingId)
            .NotEmpty()
            .WithMessage("Blessing Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessing(x))
            .WithMessage("Blessing Id does not exist.");
    }
}