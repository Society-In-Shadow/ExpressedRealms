using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.GetCharacterGoFields;

internal sealed class GetCharacterGoFieldsModelValidator
    : AbstractValidator<GetCharacterGoFieldsModel>
{
    public GetCharacterGoFieldsModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("Character does not exist.");
    }
}
