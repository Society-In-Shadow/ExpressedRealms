using ExpressedRealms.Characters.Repository;
using FluentValidation;

namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

internal sealed class GetGoCheckinInfoModelValidator : AbstractValidator<GetGoCheckinInfoModel>
{
    public GetGoCheckinInfoModelValidator(ICharacterRepository characterRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0);
    }
}
