using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class EditStatDTOValidator : AbstractValidator<EditStatDTO>
{
    public EditStatDTOValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.LevelTypeId).NotEmpty().InclusiveBetween((byte)1, (byte)7);
        RuleFor(x => x.StatTypeId).NotEmpty().IsInEnum();
    }
}
