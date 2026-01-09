using System.Collections;
using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class InvalidIdsError<T> : Error, IValidationSourcedError, IInvalidIdsError
{
    public IReadOnlyCollection<T> InvalidPermissionIds { get; }

    public InvalidIdsError(string propertyName, IEnumerable<T> invalidIds)
        : base()
    {
        InvalidPermissionIds = invalidIds.ToList();
        ValidationMessage = "One or more Ids are invalid";
        PropertyName = propertyName;
    }

    public string PropertyName { get; set; }
    public string ValidationMessage { get; set; }
    public IEnumerable InvalidIds => InvalidPermissionIds;
}
