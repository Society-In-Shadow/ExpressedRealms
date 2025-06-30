using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public static class ResultExtensions
{
    public static bool HasValidationError(this Result result, string propertyName, string errorMessage)
    {
        var validationFailure = result.Errors.OfType<FluentValidationFailure>().FirstOrDefault();
        
        if (validationFailure == null)
            return false;
            
        return validationFailure.ValidationFailures.ContainsKey(propertyName) && 
               validationFailure.ValidationFailures[propertyName].Contains(errorMessage);
    }
    
    public static bool HasValidationError(this Result result, string propertyName)
    {
        var validationFailure = result.Errors.OfType<FluentValidationFailure>().FirstOrDefault();
        
        if (validationFailure == null)
            return false;
            
        return validationFailure.ValidationFailures.ContainsKey(propertyName);
    }
}
