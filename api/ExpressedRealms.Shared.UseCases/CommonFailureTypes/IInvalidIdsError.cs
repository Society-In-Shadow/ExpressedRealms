using System.Collections;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public interface IInvalidIdsError
{
    string PropertyName { get; }
    string ValidationMessage { get; }
    IEnumerable InvalidIds { get; }
}
