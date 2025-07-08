namespace ExpressedRealms.Shared;

public interface IGenericUseCase<TResult, in T>
{
    public Task<TResult> ExecuteAsync(T model);
}

public interface IGenericUseCase<TResult>
{
    public Task<TResult> ExecuteAsync();
}
