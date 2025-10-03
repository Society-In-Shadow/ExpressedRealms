using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Expressions.Repository.Expressions;
using FluentValidation;
using JetBrains.Annotations;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet
{
    [UsedImplicitly]
    internal sealed class GetCharacterBookletModelValidator
        : AbstractValidator<GetCharacterBookletModel>
    {
        public GetCharacterBookletModelValidator(
            ICharacterRepository characterRepository,
            IExpressionRepository expressionRepository
        )
        {
            RuleFor(x => x.CharacterId)
                .NotEmpty()
                .WithMessage("Character is required.")
                .MustAsync(async (x, y) => await characterRepository.CharacterExistsAsync(x))
                .WithErrorCode("NotFound")
                .WithMessage("This is not a valid character.");
        }
    }
}
