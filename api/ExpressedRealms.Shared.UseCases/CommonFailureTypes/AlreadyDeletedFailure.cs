using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class AlreadyDeletedFailure : Error, IValidationSourcedError
{
    public AlreadyDeletedFailure(string objectName, string message)
    {
        PropertyName = objectName;
        ValidationMessage = message;
        Message = $"{objectName} was already deleted.";
    }

    public string PropertyName { get; set; }
    public string ValidationMessage { get; set; }
}
