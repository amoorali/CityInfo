using System.Linq.Expressions;

namespace CityInfo.Application.Repositories.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> Get(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>?> Find(Expression<Func<TEntity, bool>> predicate);
        Task Add(TEntity entity);
        Task Remove(TEntity entity);
    }
}
