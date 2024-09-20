using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class PlayerDTOValidator : AbstractValidator<PlayerDTO>
{
    public PlayerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
