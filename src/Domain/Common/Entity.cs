namespace Domain.Common;
public abstract class Entity
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public Guid Id { get; protected set; }
    public void ClearEvents()
        => _domainEvents.Clear();
    public void RaiseEvent(DomainEvent @event)
        => _domainEvents.Add(@event);

    protected Entity(Guid id = default) => Id = id == Guid.Empty ? Guid.NewGuid() : id;
}