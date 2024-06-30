using FluentValidation;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class PlayerDTOValidator : AbstractValidator<PlayerDTO>
{
    public PlayerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State)
            .NotEmpty()
            .MaximumLength(2)
            .MinimumLength(2)
            .Matches(
                "AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|NE"
            )
            .WithMessage("{PropertyName} is not a valid US state.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(15)
            .Matches(@"\(\d{3}\) \d{3}\-\d{4}$")
            .WithMessage("{PropertyName} must match this format: (555) 555-5555");
    }
}
