using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;

[UsedImplicitly]
internal sealed class GetBlessingLevelModelValidator : AbstractValidator<GetBlessingLevelModel>
{
    public GetBlessingLevelModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.LevelId)
            .NotEmpty()
            .WithMessage("Level Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessingLevel(x))
            .WithMessage("Level Id does not exist.");
    }
}