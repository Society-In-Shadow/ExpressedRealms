using FluentResults;

namespace ExpressedRealms.Repositories.Characters.ResultFailureTypes;

public sealed class AlreadyDeletedFailure : Error
{
    public AlreadyDeletedFailure(string objectName)
    {
        Message = $"{objectName} was already deleted.";
    }
}
