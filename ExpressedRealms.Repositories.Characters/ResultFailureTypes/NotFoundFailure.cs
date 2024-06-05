using FluentResults;

namespace ExpressedRealms.Repositories.Characters.ResultFailureTypes;

public sealed class NotFoundFailure : Error
{
    public NotFoundFailure(string objectName)
    {
        Message = $"{objectName} was not found.";
    }
}
