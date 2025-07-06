using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class NotFoundFailure : Error, IValidationSourcedError
{
    public string PropertyName { get; set; }
    public string ValidationMessage { get; set; }

    public NotFoundFailure(string objectName, string messageName)
    {
        PropertyName = objectName;
        ValidationMessage = messageName;
        Message = $"{objectName} was not found.";
    }
}
