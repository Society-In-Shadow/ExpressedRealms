using ExpressedRealms.Blessings.Repository.Blessings;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Blessings.UseCases.Blessings.CreateBlessings;

[UsedImplicitly]
internal sealed class CreateBlessingModelValidator : AbstractValidator<CreateBlessingModel>
{
    public CreateBlessingModelValidator(IBlessingRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(250)
            .WithMessage("Name must be between 1 and 250 characters.")
            .MustAsync(async (x, y) => !await repository.HasDuplicateName(x))
            .WithMessage("Blessing with this name already exists.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required.")
            .MaximumLength(50)
            .WithMessage("Type must be between 1 and 50 characters.");
        
        RuleFor(x => x.SubCategory)
            .NotEmpty()
            .WithMessage("Sub Category is required.")
            .MaximumLength(75)
            .WithMessage("Sub Category must be between 1 and 75 characters.");

    }
}
