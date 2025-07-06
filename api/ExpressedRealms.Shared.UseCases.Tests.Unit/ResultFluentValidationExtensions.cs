using System.Text;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;
using Xunit;

namespace ExpressedRealms.Shared.UseCases.Tests.Unit;

public static class ResultFluentValidationExtensions
{
    public static void MustHaveValidationError(
        this Result result,
        string propertyName,
        string? errorMessage = null
    )
    {
        Assert.False(result.IsSuccess);

        var validationFailure = result.Errors.OfType<FluentValidationFailure>().FirstOrDefault();

        if (validationFailure == null)
        {
            Assert.Fail("No validation failure");
            return;
        }

        if (
            !HandlePropertyAndReturnSuccess<IValidationSourcedError>(
                propertyName,
                validationFailure
            )
        )
            return;

        HandleMessage<IValidationSourcedError>(propertyName, errorMessage, validationFailure);

        Assert.True(true);
    }

    public static void MustHaveValidationError<T>(
        this Result<T> result,
        string propertyName,
        string? errorMessage = null
    )
    {
        Assert.False(result.IsSuccess);

        var validationFailure = result.Errors.OfType<FluentValidationFailure>().FirstOrDefault();

        if (validationFailure == null)
        {
            Assert.Fail("No validation failure");
            return;
        }

        if (
            !HandlePropertyAndReturnSuccess<IValidationSourcedError>(
                propertyName,
                validationFailure
            )
        )
            return;

        HandleMessage<IValidationSourcedError>(propertyName, errorMessage, validationFailure);

        Assert.True(true);
    }

    public static void MustHaveValidationError<T>(
        this Result result,
        string propertyName,
        string? errorMessage = null
    )
        where T : class, IValidationSourcedError
    {
        Assert.False(result.IsSuccess);

        var validationFailure = result.Errors.OfType<FluentValidationFailure>().FirstOrDefault();
        var notFoundFailure = result.Errors.OfType<T>().FirstOrDefault();

        if (validationFailure == null)
        {
            Assert.Fail("No validation failure");
            return;
        }

        if (!HandlePropertyAndReturnSuccess(propertyName, validationFailure, notFoundFailure))
            return;

        HandleMessage(propertyName, errorMessage, validationFailure, notFoundFailure);

        Assert.True(true);
    }

    public static void MustHaveValidationError<T, TResult>(
        this Result<TResult> result,
        string propertyName,
        string? errorMessage = null
    )
        where T : class, IValidationSourcedError
    {
        Assert.False(result.IsSuccess);

        var validationFailure = result.Errors.OfType<FluentValidationFailure>().FirstOrDefault();
        var validationSpecificError = result.Errors.OfType<T>().FirstOrDefault();

        if (validationFailure == null)
        {
            Assert.Fail("No validation failure");
            return;
        }

        if (
            !HandlePropertyAndReturnSuccess(
                propertyName,
                validationFailure,
                validationSpecificError
            )
        )
            return;

        HandleMessage(propertyName, errorMessage, validationFailure, validationSpecificError);
    }

    private static void HandleMessage<T>(
        string propertyName,
        string? errorMessage,
        FluentValidationFailure validationFailure,
        T? notFoundFailure = null
    )
        where T : class, IValidationSourcedError
    {
        if (notFoundFailure is not null)
        {
            if (notFoundFailure.ValidationMessage == errorMessage)
                return;
        }
        else
        {
            var hasErrorMessage = validationFailure
                .ValidationFailures[propertyName]
                .Contains(errorMessage);

            if (hasErrorMessage || string.IsNullOrWhiteSpace(errorMessage))
                return;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(
            $"The following Message was not found for property \"{propertyName}\"."
        );
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"{errorMessage}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("The available ones are:");

        // Message for the given property id
        var relatedMessages = validationFailure
            .ValidationFailures.Where(x => x.Key == propertyName)
            .ToList();

        if (notFoundFailure is not null)
        {
            relatedMessages.Add(
                new KeyValuePair<string, string[]>(
                    notFoundFailure.PropertyName,
                    new[] { notFoundFailure.ValidationMessage }
                )
            );
        }

        foreach (var (key, value) in relatedMessages)
        {
            foreach (var message in value)
            {
                stringBuilder.AppendLine($"{message} => \"{key}\"");
            }
        }

        var additionalMessages = validationFailure
            .ValidationFailures.Where(x => x.Key != propertyName)
            .ToList();

        if (additionalMessages.Count > 0)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Additional messages for other properties:");
            foreach (var (key, value) in additionalMessages)
            {
                foreach (var message in value)
                {
                    stringBuilder.AppendLine($"{message} => \"{key}\"");
                }
            }
        }

        Assert.Fail(stringBuilder.ToString());
    }

    private static bool HandlePropertyAndReturnSuccess<T>(
        string propertyName,
        FluentValidationFailure validationFailure,
        T? validationSpecificError = null
    )
        where T : class, IValidationSourcedError
    {
        if (validationSpecificError is not null)
        {
            var hasProperty = validationSpecificError.PropertyName == propertyName;
            if (hasProperty)
                return true;

            validationFailure.ValidationFailures.Add(
                new KeyValuePair<string, string[]>(
                    validationSpecificError.PropertyName,
                    new[] { validationSpecificError.ValidationMessage }
                )
            );
        }
        else
        {
            var hasProperty = validationFailure.ValidationFailures.ContainsKey(propertyName);
            if (hasProperty)
                return true;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"\"{propertyName}\" Property was not found");
        stringBuilder.AppendLine();

        if (validationFailure.ValidationFailures.Count == 0)
        {
            stringBuilder.AppendLine("No properties were found");
        }
        else
        {
            stringBuilder.AppendLine("Found these property/message pairs instead:");
            foreach (var (key, value) in validationFailure.ValidationFailures)
            {
                foreach (var message in value)
                {
                    stringBuilder.AppendLine($"\"{key}\" => {message}");
                }
            }
        }

        Assert.Fail(stringBuilder.ToString());
        return false;
    }
}
