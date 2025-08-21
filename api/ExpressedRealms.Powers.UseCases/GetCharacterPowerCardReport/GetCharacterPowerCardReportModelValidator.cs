using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public class GetCharacterPowerCardReportModelValidator : AbstractValidator<GetCharacterPowerCardReportModel>
{
    public GetCharacterPowerCardReportModelValidator(ICharacterRepository expressionRepository)
    {
        RuleFor(x => x.CharacterId)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await expressionRepository.CharacterExistsAsync(x))
            .WithMessage("The Character does not exist.");
    }
}
