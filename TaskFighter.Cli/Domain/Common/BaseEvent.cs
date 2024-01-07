using TaskFighter.Domain.Common.Interfaces;

namespace TaskFighter.Domain.Common;

public abstract class BaseEvent<T> : ITodoTaskEvent where T : Entity
{
    public int EntityId { get; private set; }
    public T Entity { get; private set; }
    public string Type { get; private set; }
    public DateTime Created { get; }

    protected BaseEvent(string type, DateTime created, T task)
    {
        Type = type;
        Created = created;
        Entity = task;
        EntityId = task?.Id ?? -1;
    }
}
