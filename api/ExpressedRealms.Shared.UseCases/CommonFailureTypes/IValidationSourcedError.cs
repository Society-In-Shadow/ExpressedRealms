namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public interface IValidationSourcedError
{
    public string PropertyName { get; set; }
    public string ValidationMessage { get; set; }
}
