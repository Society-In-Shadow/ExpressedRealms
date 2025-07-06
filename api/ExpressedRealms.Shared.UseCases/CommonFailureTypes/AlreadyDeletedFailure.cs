using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class AlreadyDeletedFailure : Error
{
    public AlreadyDeletedFailure(string objectName)
    {
        Message = $"{objectName} was already deleted.";
    }
}
