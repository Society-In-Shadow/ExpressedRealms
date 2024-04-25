using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class EditStatRequestValidator : AbstractValidator<EditStatRequest>
{
    public EditStatRequestValidator()
    {
        RuleFor(x => x.StatTypeId).NotEmpty().IsInEnum();
        RuleFor(x => x.CharacterId).NotEmpty().GreaterThan(0);
    }
}
