using FluentValidation;

namespace ExpressedRealms.UseCases.Shared;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeAValidUrl<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .Must(url =>
            {
                if (string.IsNullOrWhiteSpace(url))
                    return false;

                return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                    && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            })
            .WithMessage("{PropertyName} must be a valid URL.");
    }

    public static IRuleBuilderOptions<T, string> MustBeAValidTimeZone<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .Must(timeZoneId =>
            {
                if (string.IsNullOrWhiteSpace(timeZoneId))
                    return false;

                return TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out _);
            })
            .WithMessage("{PropertyName} is not a valid time zone.");
    }
}
