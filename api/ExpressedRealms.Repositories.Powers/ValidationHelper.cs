using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;
using FluentValidation;

namespace ExpressedRealms.Repositories.Powers;

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
        errors.AddRange(notFoundErrors.Select(x => new NotFoundFailure(x.PropertyName)));
        validationResult.Errors.RemoveAll(x => x.ErrorCode == "NotFound");

        errors.Add(new FluentValidationFailure(validationResult.ToDictionary()));

        return Result.Fail(errors);
    }
}
