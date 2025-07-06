using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class FluentValidationFailure(IDictionary<string, string[]> validationFailures)
    : Error
{
    public IDictionary<string, string[]> ValidationFailures { get; } = validationFailures;
}
