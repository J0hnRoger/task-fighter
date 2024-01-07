using TaskFighter.Domain.Common;

namespace TaskFighter.Domain.EventsManagement;

public static class CalendarEvents
{
    public static BaseEvent<Entity> CreateEventEvent(TodoEvent task)
    {
        return new EventCreatedEvent(task, DateTime.Now);
    }

    public static EventUpdatedEvent UpdateEventEvent(TodoEvent task)
    {
        return new EventUpdatedEvent(task, DateTime.Now);
    }

    public static EventDeletedEvent DeleteEventEvent(TodoEvent task)
    {
        return new EventDeletedEvent(task, DateTime.Now);
    }

    public static EventStartedEvent StartEventEvent(TodoEvent task)
    {
        return new EventStartedEvent(task, DateTime.Now);
    }

    public static EventStoppedEvent StopEventEvent(TodoEvent task)
    {
        return new EventStoppedEvent(task, DateTime.Now);
    }

    public static EventContinuedEvent ContinueEventEvent(TodoEvent task)
    {
        return new EventContinuedEvent(task, DateTime.Now);
    }

    public static EventCompletedEvent EndEventEvent(TodoEvent task)
    {
        return new EventCompletedEvent(task, DateTime.Now);
    }

    public static EventMigrated MigrateEventEvent(TodoEvent task)
    {
        return new EventMigrated(task, DateTime.Now);
    }

}

public class EventCreatedEvent : BaseEvent<Entity>
{
    public TodoEvent Event { get; private set; }

    public EventCreatedEvent(TodoEvent task, DateTime created) : base("CreateEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventDeletedEvent : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventDeletedEvent(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventUpdatedEvent : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventUpdatedEvent(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventStartedEvent : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventStartedEvent(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventStoppedEvent : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventStoppedEvent(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventContinuedEvent : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventContinuedEvent(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventMigrated : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventMigrated(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}

public class EventCompletedEvent : BaseEvent<TodoEvent>
{
    public TodoEvent Event { get; private set; }

    public EventCompletedEvent(TodoEvent task, DateTime created) : base("RemoveEventEvent", created, task)
    {
        Event = task;
    }
}