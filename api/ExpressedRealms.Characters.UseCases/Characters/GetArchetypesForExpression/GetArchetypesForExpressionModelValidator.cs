using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.GetArchetypesForExpression;

internal sealed class GetArchetypesForExpressionModelValidator : AbstractValidator<GetArchetypesForExpressionModel>
{
    public GetArchetypesForExpressionModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Expression Id is required.")
            .MustAsync(async (x, y) => await characterRepository.ExpressionExistsAsync(x))
            .WithMessage("The Expression Id does not exist.");
    }
}
