using FluentResults;

namespace ExpressedRealms.Repositories.Shared.CommonFailureTypes;

public sealed class NotFoundFailure : Error
{
    public NotFoundFailure(string objectName)
    {
        Message = $"{objectName} was not found.";
    }
}
