using System.Text;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Server.Shared;

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
        {
            var stringBuilder = new StringBuilder();
            result.Reasons.ForEach(x => stringBuilder.AppendLine(x.Message));
            stringBuilder.AppendLine("The following result error(s) was not handled:");
            foreach (var error in result.Errors)
            {
                stringBuilder.AppendLine("Message:");
                stringBuilder.AppendLine(error.Message);
                stringBuilder.AppendLine("Reasons:");
                foreach (var reason in error.Reasons)
                {
                    stringBuilder.AppendLine(reason.Message);
                }
            }

            throw new NotImplementedException(stringBuilder.ToString());
        }
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
        typedResults = TypedResults.ValidationProblem(new Dictionary<string, string[]>());

        var hasFluent = result.HasError<FluentValidationFailure>();
        var hasInvalidIds = result.Errors.OfType<IInvalidIdsError>().Any();

        if (!hasFluent && !hasInvalidIds)
            return false;

        typedResults = TypedResults.ValidationProblem(GetValidationFailure(result.Errors));
        return true;
    }

    public static bool HasValidationError<T>(
        this Result<T> result,
        out ValidationProblem typedResults
    )
    {
        typedResults = TypedResults.ValidationProblem(new Dictionary<string, string[]>());
        
        var hasFluent = result.HasError<FluentValidationFailure>();
        var hasInvalidIds = result.Errors.OfType<IInvalidIdsError>().Any();

        if (!hasFluent && !hasInvalidIds)
            return false;

        typedResults = TypedResults.ValidationProblem(GetValidationFailure(result.Errors));
        return true;
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

    public static bool HasInsufficientXP(
        this Result result,
        out BadRequest<string> insufficientXPMessage
    )
    {
        insufficientXPMessage = TypedResults.BadRequest("This is not a valid error");

        if (!result.HasError<NotEnoughXPFailure>())
            return false;

        var xpResults = (NotEnoughXPFailure)result.Errors[0];
        insufficientXPMessage = TypedResults.BadRequest(
            "You don't have enough XP to do that.  You have "
                + xpResults.AvailableXP
                + " points available.  You tried to spend "
                + xpResults.AmountTryingToSpend
                + " points."
        );
        return true;
    }

    public static bool HasInsufficientXP<T>(
        this Result<T> result,
        out BadRequest<string> insufficientXPMessage
    )
    {
        insufficientXPMessage = TypedResults.BadRequest("This is not a valid error");

        if (!result.HasError<NotEnoughXPFailure>())
            return false;

        var xpResults = (NotEnoughXPFailure)result.Errors[0];
        insufficientXPMessage = TypedResults.BadRequest(
            "You don't have enough XP to do that.  You have "
                + xpResults.AvailableXP
                + " points available.  You tried to spend "
                + xpResults.AmountTryingToSpend
                + " points."
        );
        return true;
    }

    private static IDictionary<string, string[]> GetValidationFailure(List<IError> errors)
    {
        if(errors.Count == 0)
            return new Dictionary<string, string[]>();
        
        var fluentFailure = errors.OfType<FluentValidationFailure>().FirstOrDefault();

        var basicValidations = fluentFailure != null
            ? fluentFailure.ValidationFailures
            : new Dictionary<string, string[]>();
        
        foreach (var error in errors.OfType<IInvalidIdsError>())
        {
            var existing = basicValidations.TryGetValue(error.PropertyName, out var validation)
                ? validation
                : Array.Empty<string>();

            basicValidations[error.PropertyName] = existing
                .Concat(new[] { error.ValidationMessage }
                    .Concat(error.InvalidIds.Cast<object>().Select(id => id.ToString())))
                .ToArray()!;
        }
        
        return basicValidations;
    }
}
