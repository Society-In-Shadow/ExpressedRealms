using FluentValidation;

namespace ExpressedRealms.Repositories.Characters.Stats.DTOs;

internal sealed class GetDetailedStatInfoDtoValidator : AbstractValidator<GetDetailedStatInfoDto>
{
    public GetDetailedStatInfoDtoValidator()
    {
        RuleFor(x => x.StatTypeId).NotEmpty().IsInEnum();
        RuleFor(x => x.CharacterId).NotEmpty().GreaterThan(0);
    }
}
