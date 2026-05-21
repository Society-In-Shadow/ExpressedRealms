using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.EditCharacterGoFields;

internal sealed class EditCharacterGoFieldsModelValidator
    : AbstractValidator<EditCharacterGoFieldsModel>
{
    public EditCharacterGoFieldsModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("Character does not exist.");

        RuleFor(x => x.WealthLevel)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Prima Level must be at least 0.");

        RuleFor(x => x.PrimaFragments)
            .InclusiveBetween(0, 5)
            .WithMessage("Prima fragments must be at least 0.");

        RuleFor(x => x.Motes)
            .InclusiveBetween(-7, 7)
            .WithMessage("Prima motes must be between -7 and 7.");

        RuleFor(x => x.VoidFragments)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Void fragments must be at least 0.");
    }
}
