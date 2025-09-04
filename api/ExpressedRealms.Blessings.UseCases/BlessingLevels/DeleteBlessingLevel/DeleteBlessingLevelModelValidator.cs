using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.DeleteBlessingLevel;

[UsedImplicitly]
internal sealed class DeleteBlessingLevelModelValidator
    : AbstractValidator<DeleteBlessingLevelModel>
{
    public DeleteBlessingLevelModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.LevelId).NotEmpty().WithMessage("Level Id is required.");

        RuleFor(x => x)
            .MustAsync(
                async (x, y) => await repository.IsExistingBlessingLevel(x.BlessingId, x.LevelId)
            )
            .WithName(nameof(DeleteBlessingLevelModel.LevelId))
            .WithMessage("Level Id does not exist.");

        RuleFor(x => x.BlessingId)
            .NotEmpty()
            .WithMessage("Blessing Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessing(x))
            .WithMessage("Blessing Id does not exist.");
    }
}
