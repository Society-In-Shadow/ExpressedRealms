using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Global.API.Search.GlobalSearch;

[UsedImplicitly]
internal sealed class GlobalSearchUseCaseModelValidator : AbstractValidator<GlobalSearchUseCaseModel>
{
    public GlobalSearchUseCaseModelValidator()
    {
        RuleFor(x => x.Search)
            .NotEmpty()
            .WithMessage("Search is required.");
    }
}