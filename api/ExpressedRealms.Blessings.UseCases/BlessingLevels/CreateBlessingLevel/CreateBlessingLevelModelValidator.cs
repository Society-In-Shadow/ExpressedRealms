using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.CreateBlessingLevel;

[UsedImplicitly]
internal sealed class CreateBlessingLevelModelValidator : AbstractValidator<CreateBlessingLevelModel>
{
    public CreateBlessingLevelModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.Level)
            .NotEmpty()
            .WithMessage("Level is required.")
            .MaximumLength(25)
            .WithMessage("Name must be between 1 and 25 characters.")
            .MustAsync(async (x, y) => !await repository.HasDuplicateName(x))
            .WithMessage("Blessing Level with this level already exists.")
            .Matches(@"^\d+ pt\.$")
            .WithMessage("Level must be in the format of '123 pt.'");
        
        RuleFor(x => x)
            .MustAsync(async (x, y) => !await repository.HasDuplicateLevelName(x.BlessingId, x.Level))
            .WithName(nameof(CreateBlessingLevelModel.Level))
            .WithMessage("Blessing Level with this level already exists.");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");
        
        RuleFor(x => x.XpCost)
            .NotEmpty()
            .WithMessage("Xp Cost is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Xp Cost must be greater than or equal to 0.");
        
        RuleFor(x => x.XpGain)
            .NotEmpty()
            .WithMessage("Xp Gain is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Xp Gain must be greater than or equal to 0.");
        
        RuleFor(x => x.BlessingId)
            .NotEmpty()
            .WithMessage("Blessing Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessing(x))
            .WithMessage("Blessing does not exist.");
    }
}