using ExpressedRealms.Expressions.Repository.Factions;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFaction;

[UsedImplicitly]
internal sealed class GetFactionModelValidator : AbstractValidator<GetFactionModel>
{
    public GetFactionModelValidator(IFactionRepository repository)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
