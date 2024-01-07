namespace TaskFighter.Domain.Common;

public abstract class Entity
{
    public IReadOnlyList<BaseEvent<Entity>> DomainEvents => _domainEvents;
    protected List<BaseEvent<Entity>> _domainEvents = new List<BaseEvent<Entity>>();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public int Id { get; set; }
    public DateTime Created { get; set; }
}