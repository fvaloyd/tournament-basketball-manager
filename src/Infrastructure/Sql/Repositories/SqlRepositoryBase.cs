using Domain.Common;
using System.Linq.Expressions;
using Infrastructure.Sql.Context;

namespace Infrastructure.Sql.Repositories;
public class SqlRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
{
    private readonly TournamentBasketballManagerDbContext _db;

    public SqlRepositoryBase(TournamentBasketballManagerDbContext db) => _db = db;

    public void Create(TEntity entity) => _db.Set<TEntity>().Add(entity);

    public void Delete(TEntity entity) => _db.Set<TEntity>().Remove(entity);

    public IQueryable<TEntity> FindAll() => _db.Set<TEntity>().AsQueryable();

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> filter) => _db.Set<TEntity>().Where(filter).AsQueryable();

    public void Update(TEntity entity) => _db.Set<TEntity>().Update(entity);
}