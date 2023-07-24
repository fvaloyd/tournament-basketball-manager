using Domain;

namespace Presentation.Web.Common;

public abstract class EntityBaseState<TEntity> where TEntity : IEntity
{
    private TEntity _current = default!;
    private List<TEntity> _entities = new();

    public TEntity Current => _current;
    public IReadOnlyCollection<TEntity> Entites => _entities.AsReadOnly();

    public void SetCurrent(Guid id)
        => _current = _entities.First(o => o.Id == id);
    public void Add(TEntity entity)
        => _entities.Add(entity);

    public abstract void AddEntity(object formModel, Guid id);

    public void InitializeEntities(IEnumerable<TEntity> entities)
        => _entities = entities.ToList();
}
