using FluentResults;

namespace ExpressedRealms.Repositories.Shared.CommonFailureTypes;

public sealed class AlreadyDeletedFailure : Error
{
    public AlreadyDeletedFailure(string objectName)
    {
        Message = $"{objectName} was already deleted.";
    }
}
