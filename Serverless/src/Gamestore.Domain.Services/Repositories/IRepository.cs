namespace Gamestore.Domain.Services.Repositories;

public interface IRepository
{
}

public interface IGenericRepository<TEntity, in TKey> : IRepository
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
}