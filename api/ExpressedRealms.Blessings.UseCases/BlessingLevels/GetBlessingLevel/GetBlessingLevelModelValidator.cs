using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;

[UsedImplicitly]
internal sealed class GetBlessingLevelModelValidator : AbstractValidator<GetBlessingLevelModel>
{
    public GetBlessingLevelModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessing(x))
            .WithMessage("Blessing does not exist.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be between 1 and 250 characters.")
            .MustAsync(async (x, y) => !await repository.HasDuplicateName(x))
            .WithMessage("Blessing with this name already exists.");
    }
}