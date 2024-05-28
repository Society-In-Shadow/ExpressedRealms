using FluentResults;

namespace ExpressedRealms.Repositories.Characters;

public class FluentValidationFailure : Error
{
    public IDictionary<string, string[]> ValidationFailures;
    public FluentValidationFailure(IDictionary<string, string[]> validationFailures)
    {
        ValidationFailures = validationFailures;
    }
}