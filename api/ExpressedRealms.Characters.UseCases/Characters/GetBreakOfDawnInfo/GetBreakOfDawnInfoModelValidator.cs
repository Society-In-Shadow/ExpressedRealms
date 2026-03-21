using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.GetBreakOfDawnInfo;

internal sealed class GetBreakOfDawnInfoModelValidator : AbstractValidator<GetBreakOfDawnInfoModel>
{
    public GetBreakOfDawnInfoModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Character Id is required.")
            .MustAsync(async (x, y) => await characterRepository.FindCharacterAsync(x) is not null)
            .WithMessage("The Character Id does not exist.");
    }
}
