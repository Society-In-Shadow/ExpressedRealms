using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.DeleteBlessingLevel;

[UsedImplicitly]
internal sealed class DeleteBlessingLevelModelValidator : AbstractValidator<DeleteBlessingLevelModel>
{
    public DeleteBlessingLevelModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.LevelId)
            .NotEmpty()
            .WithMessage("Level Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessingLevel(x))
            .WithMessage("Level Id does not exist.");
    }
}