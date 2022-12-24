using TaskSamurai.Domain.Common;

namespace TaskSamurai.Domain.TasksManagement;

public static class TaskEvents
{
    public static BaseEvent<Entity> CreateTaskEvent(TodoTask task)
    {
        return new TaskCreatedEvent(task, DateTime.Now);
    }

    public static TaskUpdatedEvent UpdateTaskEvent(TodoTask task)
    {
        return new TaskUpdatedEvent(task, DateTime.Now);
    }

    public static TaskDeletedEvent DeleteTaskEvent(TodoTask task)
    {
        return new TaskDeletedEvent(task, DateTime.Now);
    }

    public static TaskStartedEvent StartTaskEvent(TodoTask task)
    {
        return new TaskStartedEvent(task, DateTime.Now);
    }

    public static TaskStoppedEvent StopTaskEvent(TodoTask task)
    {
        return new TaskStoppedEvent(task, DateTime.Now);
    }

    public static TaskContinuedEvent ContinueTaskEvent(TodoTask task)
    {
        return new TaskContinuedEvent(task, DateTime.Now);
    }

    public static TaskCompletedEvent EndTaskEvent(TodoTask task)
    {
        return new TaskCompletedEvent(task, DateTime.Now);
    }

    public static TaskMigrated MigrateTaskEvent(TodoTask task)
    {
        return new TaskMigrated(task, DateTime.Now);
    }
}

public class TaskCreatedEvent : BaseEvent<Entity>
{
    public TaskCreatedEvent(TodoTask ritual, DateTime created) : base("CreateTaskEvent", created, ritual) { }
}

public class TaskDeletedEvent : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskDeletedEvent(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class TaskUpdatedEvent : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskUpdatedEvent(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class TaskStartedEvent : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskStartedEvent(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class TaskStoppedEvent : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskStoppedEvent(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class TaskContinuedEvent : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskContinuedEvent(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class TaskMigrated : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskMigrated(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}

public class TaskCompletedEvent : BaseEvent<TodoTask>
{
    public TodoTask Ritual { get; private set; }

    public TaskCompletedEvent(TodoTask ritual, DateTime created) : base("RemoveTaskEvent", created, ritual)
    {
        Ritual = ritual;
    }
}