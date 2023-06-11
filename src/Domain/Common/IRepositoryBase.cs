using System.Linq.Expressions;

namespace Domain.Common;
public interface IRepositoryBase<TEntity> 
    where TEntity : class, IEntity
{
    IQueryable<TEntity> FindAll();
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> filter);
    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}