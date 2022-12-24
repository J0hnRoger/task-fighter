using TaskSamurai.Domain.Common.Interfaces;

namespace TaskSamurai.Domain.Common;

public abstract class BaseEvent<T> : ISamuraiTaskEvent where T : Entity
{
    public int EntityId { get; private set; }
    public T Entity { get; private set; }
    public string Type { get; private set; }
    public DateTime Created { get; }

    protected BaseEvent(string type, DateTime created, T ritual)
    {
        Type = type;
        Created = created;
        Entity = ritual;
        EntityId = ritual.Id;
    }
}
