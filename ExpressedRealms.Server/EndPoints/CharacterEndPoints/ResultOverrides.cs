using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints;

public static class ResultOverrides
{
    public static void ThrowIfErrorNotHandled(this Result result)
    {
        if (result.IsFailed)
            throw new NotImplementedException("A Result Error Was Not Handled");
    }

    public static void ThrowIfErrorNotHandled<T>(this Result<T> result)
    {
        if (result.IsFailed)
            throw new NotImplementedException("A Result Error Was Not Handled");
    }

    public static bool HasNotFound(this Result result, out NotFound typedResults)
    {
        typedResults = TypedResults.NotFound();
        return result.HasError<NotFoundFailure>();
    }

    public static bool HasNotFound<T>(this Result<T> result, out NotFound typedResults)
    {
        typedResults = TypedResults.NotFound();
        return result.HasError<NotFoundFailure>();
    }

    public static bool HasValidationError(this Result result, out ValidationProblem typedResults)
    {
        typedResults = TypedResults.ValidationProblem(GetValidationFailure(result.Errors));
        return result.HasError<FluentValidationFailure>();
    }

    public static bool HasValidationError<T>(
        this Result<T> result,
        out ValidationProblem typedResults
    )
    {
        typedResults = TypedResults.ValidationProblem(GetValidationFailure(result.Errors));
        return result.HasError<FluentValidationFailure>();
    }

    public static bool HasBeenDeletedAlready(
        this Result result,
        out StatusCodeHttpResult typedResults
    )
    {
        typedResults = TypedResults.StatusCode(410);
        return result.HasError<AlreadyDeletedFailure>();
    }

    public static bool HasBeenDeletedAlready<T>(
        this Result<T> result,
        out StatusCodeHttpResult typedResults
    )
    {
        typedResults = TypedResults.StatusCode(410);
        return result.HasError<AlreadyDeletedFailure>();
    }

    private static IDictionary<string, string[]> GetValidationFailure(List<IError> errors)
    {
        return ((FluentValidationFailure)errors[0]).ValidationFailures;
    }
}
