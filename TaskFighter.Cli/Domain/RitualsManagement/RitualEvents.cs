using TaskFighter.Domain.Common;

namespace TaskFighter.Domain.RitualsManagement;

public static class RitualEvents
{
    public static BaseEvent<Entity> CreateRitualEvent(Ritual task)
    {
        return new RitualCreatedEvent(task, DateTime.Now);
    }

    public static RitualUpdatedEvent UpdateRitualEvent(Ritual task)
    {
        return new RitualUpdatedEvent(task, DateTime.Now);
    }

    public static RitualDeletedEvent DeleteRitualEvent(Ritual task)
    {
        return new RitualDeletedEvent(task, DateTime.Now);
    }

    public static RitualAppliedEvent StartRitualEvent(Ritual task)
    {
        return new RitualAppliedEvent(task, DateTime.Now);
    }
    public static RitualCompletedEvent EndRitualEvent(Ritual task)
    {
        return new RitualCompletedEvent(task, DateTime.Now);
    }
}

public class RitualCreatedEvent : BaseEvent<Entity>
{
    public RitualCreatedEvent(Ritual task, DateTime created) : base("CreateRitualEvent", created, task) { }
}

public class RitualDeletedEvent : BaseEvent<Ritual>
{
    public Ritual Task { get; private set; }

    public RitualDeletedEvent(Ritual task, DateTime created) : base("RemoveRitualEvent", created, task)
    {
        Task = task;
    }
}

public class RitualUpdatedEvent : BaseEvent<Ritual>
{
    public Ritual Task { get; private set; }

    public RitualUpdatedEvent(Ritual task, DateTime created) : base("RemoveRitualEvent", created, task)
    {
        Task = task;
    }
}

public class RitualAppliedEvent : BaseEvent<Ritual>
{
    public Ritual Task { get; private set; }

    public RitualAppliedEvent (Ritual task, DateTime created) : base("RitualAppliedEvent", created, task)
    {
        Task = task;
    }
}

public class RitualCompletedEvent : BaseEvent<Ritual>
{
    public Ritual Task { get; private set; }

    public RitualCompletedEvent(Ritual task, DateTime created) : base("RemoveRitualEvent", created, task)
    {
        Task = task;
    }
}
