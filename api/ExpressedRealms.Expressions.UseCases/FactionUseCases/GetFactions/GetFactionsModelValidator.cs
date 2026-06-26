using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;

[UsedImplicitly]
internal sealed class GetFactionsModelValidator : AbstractValidator<GetFactionsModel>
{
    public GetFactionsModelValidator()
    {
        RuleFor(x => x.ExpressionId).NotEmpty().WithMessage("Expression Id is required.");
    }
}
