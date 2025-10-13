using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;

public class GetCharacterSheetReportModelValidator : AbstractValidator<GetCharacterSheetReportModel>
{
    public GetCharacterSheetReportModelValidator(ICharacterRepository expressionRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await expressionRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}
