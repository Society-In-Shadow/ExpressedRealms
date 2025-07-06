using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class NotFoundFailure : Error
{
    public NotFoundFailure(string objectName)
    {
        Message = $"{objectName} was not found.";
    }
}
