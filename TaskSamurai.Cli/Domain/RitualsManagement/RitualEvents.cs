using TaskSamurai.Domain.Common;

namespace TaskSamurai.Domain.RitualsManagement;

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
    public RitualCreatedEvent(Ritual ritual, DateTime created) : base("CreateRitualEvent", created, ritual) { }
}

public class RitualDeletedEvent : BaseEvent<Ritual>
{
    public Ritual Ritual { get; private set; }

    public RitualDeletedEvent(Ritual ritual, DateTime created) : base("RemoveRitualEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class RitualUpdatedEvent : BaseEvent<Ritual>
{
    public Ritual Ritual { get; private set; }

    public RitualUpdatedEvent(Ritual ritual, DateTime created) : base("RemoveRitualEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class RitualAppliedEvent : BaseEvent<Ritual>
{
    public Ritual Ritual { get; private set; }

    public RitualAppliedEvent (Ritual ritual, DateTime created) : base("RitualAppliedEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class RitualCompletedEvent : BaseEvent<Ritual>
{
    public Ritual Ritual { get; private set; }

    public RitualCompletedEvent(Ritual ritual, DateTime created) : base("RemoveRitualEvent", created, ritual)
    {
        Ritual = ritual;
    }
}
