using ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;
using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.Create;

public class CreatePlayerRequestValidator : AbstractValidator<PlayerDTO>
{
    public CreatePlayerRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
