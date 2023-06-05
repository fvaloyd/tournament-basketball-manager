using System.Linq.Expressions;

namespace Domain.Common;
public interface IRepositoryBase<TEntity> where TEntity : Entity
{
    Task<IQueryable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken);
    Task<IQueryable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken);
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}