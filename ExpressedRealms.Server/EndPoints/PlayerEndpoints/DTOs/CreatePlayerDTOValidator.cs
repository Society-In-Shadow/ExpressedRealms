using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.DTOs;

public class CreatePlayerDTOValidator : AbstractValidator<CreatePlayerDTO>
{
    public CreatePlayerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(2).MinimumLength(2);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
    }
}