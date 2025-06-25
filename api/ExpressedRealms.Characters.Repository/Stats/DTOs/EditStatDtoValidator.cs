using FluentValidation;

namespace ExpressedRealms.Characters.Repository.Stats.DTOs;

public class EditStatDtoValidator : AbstractValidator<EditStatDto>
{
    public EditStatDtoValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.LevelTypeId).NotEmpty().InclusiveBetween((byte)1, (byte)7);
        RuleFor(x => x.StatTypeId).NotEmpty().IsInEnum();
    }
}
