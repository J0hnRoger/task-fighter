using TaskFighter.Domain.Common;

namespace TaskFighter.Domain.TasksManagement;

public static class TaskEvents
{
    public static BaseEvent<Entity> CreateTaskEvent(TodoTask task)
    {
        return new TaskCreatedEvent(task, DateTime.Now);
    }

    public static BaseEvent<Entity> DeleteTaskEvent(TodoTask task)
    {
        return new TaskDeletedEvent(task, DateTime.Now);
    }
    
    public static BaseEvent<Entity> EndTaskEvent(TodoTask task)
    {
        return new TaskCompletedEvent(task, DateTime.Now);
    }

    public static TaskUpdatedEvent UpdateTaskEvent(TodoTask task)
    {
        return new TaskUpdatedEvent(task, DateTime.Now);
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

    public static TaskMigrated MigrateTaskEvent(TodoTask task)
    {
        return new TaskMigrated(task, DateTime.Now);
    }
}

public class TaskCreatedEvent : BaseEvent<Entity>
{
    public TaskCreatedEvent(TodoTask task, DateTime created) : base("CreateTaskEvent", created, task) { }
}

public class TaskDeletedEvent : BaseEvent<Entity>
{
    public TodoTask Task { get; private set; }

    public TaskDeletedEvent(TodoTask task, DateTime created) : base("RemoveTaskEvent", created, task)
    {
        Task = task;
    }
}

public class TaskUpdatedEvent : BaseEvent<TodoTask>
{
    public TodoTask Task { get; private set; }

    public TaskUpdatedEvent(TodoTask task, DateTime created) : base("RemoveTaskEvent", created, task)
    {
        Task = task;
    }
}

public class TaskStartedEvent : BaseEvent<Entity>
{
    public TodoTask Task { get; private set; }

    public TaskStartedEvent(TodoTask task, DateTime created) : base("StartTaskEvent", created, task)
    {
        Task = task;
    }
}

public class TaskStoppedEvent : BaseEvent<TodoTask>
{
    public TodoTask Task { get; private set; }

    public TaskStoppedEvent(TodoTask task, DateTime created) : base("RemoveTaskEvent", created, task)
    {
        Task = task;
    }
}

public class TaskContinuedEvent : BaseEvent<TodoTask>
{
    public TodoTask Task { get; private set; }

    public TaskContinuedEvent(TodoTask task, DateTime created) : base("RemoveTaskEvent", created, task)
    {
        Task = task;
    }
}

public class TaskMigrated : BaseEvent<TodoTask>
{
    public TodoTask Task { get; private set; }

    public TaskMigrated(TodoTask task, DateTime created) : base("RemoveTaskEvent", created, task)
    {
        Task = task;
    }
}

public class TaskCompletedEvent : BaseEvent<Entity>
{
    public TodoTask Task { get; private set; }

    public TaskCompletedEvent(TodoTask task, DateTime created) : base("CompleteTaskEvent", created, task)
    {
        Task = task;
    }
}