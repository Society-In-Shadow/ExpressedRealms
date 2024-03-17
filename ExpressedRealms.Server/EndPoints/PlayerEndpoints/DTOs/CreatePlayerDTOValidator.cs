using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class CreatePlayerDTOValidator : AbstractValidator<CreatePlayerDTO>
{
    public CreatePlayerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(2).MinimumLength(2);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(15)
            .Matches(@"\(\d{3}\) \d{3}\-\d{4}$")
            .WithMessage("{PropertyName} must match this format: (555) 555-5555");
    }
}
