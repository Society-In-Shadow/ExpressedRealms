using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class UnauthorizedError(string message = "Unauthorized") : Error(message);
