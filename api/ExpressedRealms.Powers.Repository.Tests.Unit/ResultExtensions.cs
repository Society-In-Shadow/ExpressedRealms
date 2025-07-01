using System.Text;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public static class ResultExtensions
{
    public static void HasValidationError(
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

        if (!HandlePropertyAndReturnSuccess(propertyName, validationFailure))
            return;

        HandleMessage(propertyName, errorMessage, validationFailure);
    }

    private static void HandleMessage(
        string propertyName,
        string? errorMessage,
        FluentValidationFailure validationFailure
    )
    {
        var hasErrorMessage = validationFailure
            .ValidationFailures[propertyName]
            .Contains(errorMessage);

        if (hasErrorMessage || string.IsNullOrWhiteSpace(errorMessage))
            return;

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(
            $"The following Message was not found for property \"{propertyName}\"."
        );
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"{errorMessage}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("The available ones are:");

        // Message for the given property id
        var relatedMessages = validationFailure.ValidationFailures.Where(x =>
            x.Key == propertyName
        );

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

    private static bool HandlePropertyAndReturnSuccess(
        string propertyName,
        FluentValidationFailure validationFailure
    )
    {
        var hasProperty = validationFailure.ValidationFailures.ContainsKey(propertyName);
        if (hasProperty)
            return true;

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
