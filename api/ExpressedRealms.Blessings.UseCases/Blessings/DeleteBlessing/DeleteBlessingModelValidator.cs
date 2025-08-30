using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.Blessings.DeleteBlessing;

[UsedImplicitly]
internal sealed class DeleteBlessingModelValidator : AbstractValidator<DeleteBlessingModel>
{
    public DeleteBlessingModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.")
            .MustAsync(async (x, y) => await repository.IsExistingBlessing(x))
            .WithMessage("NotFound")
            .WithMessage("This blessing was not found.");
    }
}
