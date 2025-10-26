namespace ExpressedRealms.Shared;

public interface IGenericRepository
{
    Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class;
}
