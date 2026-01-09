using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;

namespace ExpressedRealms.UseCases.Shared;

public static class ValidationHelper
{
    public static async Task<Result<TModel>> ValidateAndHandleErrorsAsync<TModel>(
        IValidator<TModel> validator,
        TModel model,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(model, cancellationToken);

        if (validationResult.IsValid)
        {
            return Result.Ok(model);
        }

        var errors = new List<IError>();

        var notFoundErrors = validationResult.Errors.Where(x => x.ErrorCode == "NotFound");
        errors.AddRange(
            notFoundErrors.Select(x => new NotFoundFailure(x.PropertyName, x.ErrorMessage))
        );
        validationResult.Errors.RemoveAll(x => x.ErrorCode == "NotFound");

        var alreadyDeleted = validationResult.Errors.Where(x => x.ErrorCode == "AlreadyDeleted");
        errors.AddRange(
            alreadyDeleted.Select(x => new AlreadyDeletedFailure(x.PropertyName, x.ErrorMessage))
        );
        validationResult.Errors.RemoveAll(x => x.ErrorCode == "AlreadyDeleted");

        var invalidIdsFailures = validationResult
            .Errors.Where(x => x.ErrorCode == "InvalidIds" && x.CustomState is IError)
            .Select(x => (IError)x.CustomState);
        errors.AddRange(invalidIdsFailures);
        validationResult.Errors.RemoveAll(x => x.ErrorCode == "InvalidIds");

        if (validationResult.Errors.Count != 0)
            errors.Add(new FluentValidationFailure(validationResult.ToDictionary()));

        return Result.Fail(errors);
    }

    public static ValidationFailure AddInvalidIdsFailure<T>(
        string permissionName,
        List<T> invalidPermissions
    )
    {
        return new ValidationFailure(permissionName, "One or more Ids are invalid")
        {
            CustomState = new InvalidIdsError<T>(permissionName, invalidPermissions),
            ErrorCode = "InvalidIds",
        };
    }
}
