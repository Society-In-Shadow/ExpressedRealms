using FluentResults;

namespace ExpressedRealms.Repositories.Shared.CommonFailureTypes;

public sealed class FluentValidationFailure : Error
{
    public IDictionary<string, string[]> ValidationFailures;

    public FluentValidationFailure(IDictionary<string, string[]> validationFailures)
    {
        ValidationFailures = validationFailures;
    }
}
