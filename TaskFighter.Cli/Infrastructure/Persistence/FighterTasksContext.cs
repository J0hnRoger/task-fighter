using Newtonsoft.Json;
using TaskFighter.Domain;
using TaskFighter.Domain.Common;
using TaskFighter.Domain.EventsManagement;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Infrastructure.Persistence;


public class FighterTasksContext : IFighterTaskContext 
{
    private readonly TaskFighterConfig _dbConfig;

    private readonly List<BaseEvent<Entity>> _domainEvents = new();
    private readonly DailyTodo _currentDay;

    private DailyTodoLists _todoLists { get; set; }
    private List<TodoTask> _backlog { get; set; }
    private List<TodoEvent> _events { get; set; }
    private List<TodoTask> _completed { get; set; }
    
    public IReadOnlyList<TodoTask> Backlog => _backlog;

    public DailyTodoLists DailyTodoLists => _todoLists;
    public IReadOnlyList<TodoTask> Tasks => _currentDay.Tasks;
    public DailyTodo DailyTodo => _currentDay;
    public DailyTodo Tomorrow => GetOrCreateTodoList(DateTime.Today.AddDays(1));
    
    public IReadOnlyList<TodoEvent> Events => _events;

    public string Context => _dbConfig.Context;

    /// <summary>
    /// Unit of Work for recording tasks
    /// </summary>
    /// <param name="dbConfig">Config path, for index</param>
    public FighterTasksContext(TaskFighterConfig dbConfig)
    {
        _dbConfig = dbConfig;

        string backlogPath = Load(_dbConfig.BackLogPath);
        string completedPath = Load(_dbConfig.CompletedPath);
        string dailyTodoPath = Load(_dbConfig.TodosPath);

        _backlog = JsonConvert.DeserializeObject<List<TodoTask>>(backlogPath
            , new TodoTaskStatusJsonConverter(typeof(TodoTaskStatus))) ?? new List<TodoTask>();

        _completed = JsonConvert.DeserializeObject<List<TodoTask>>(completedPath
            , new TodoTaskStatusJsonConverter(typeof(TodoTaskStatus))) ?? new List<TodoTask>();
        
        _todoLists = JsonConvert.DeserializeObject<DailyTodoLists>(dailyTodoPath,
                         new TodoTaskStatusJsonConverter(typeof(TodoTaskStatus)))
                     ?? new DailyTodoLists();

        var today = DateTime.Today;

        var dailyTodo = GetOrCreateTodoList(today);
        _currentDay = dailyTodo;

        string calendarFileContent = Load(_dbConfig.CalendarPath);
        _events = JsonConvert.DeserializeObject<List<TodoEvent>>(calendarFileContent) ?? new List<TodoEvent>();
    }

    private string Load(string todoPath)
    {
        string fileContent = File.ReadAllText(todoPath);
        return fileContent;
    }

    public void SetContext(string context)
    {
        _dbConfig.Context = context.ToLower();
    }

    public void Migrate(TodoTask migrateTask)
    {
        var tomorrow = DateTime.Today.AddDays(1);
        var dailyTodo = GetOrCreateTodoList(tomorrow);
        migrateTask.Status = TodoTaskStatus.Planned;
        dailyTodo.Tasks.Add(migrateTask);
        SaveChanges(); 
    }
    
    private DailyTodo GetOrCreateTodoList(DateTime day)
    {
        var dailyTodo = _todoLists.GetTodoList(day);
        if (dailyTodo == null)
        {
            dailyTodo = new DailyTodo() {Date = day, Tasks = new List<TodoTask>(), Opened = false };
            _todoLists.Days.Add(dailyTodo);
            File.WriteAllText(_dbConfig.TodosPath, JsonConvert.SerializeObject(_todoLists, Formatting.Indented));
        }
        return dailyTodo;
    }

    public TodoEvent AddEvent(TodoEvent newTodoEvent)
    {
        newTodoEvent.Id = ++_dbConfig.CurrentIndex;
        _events.Add(newTodoEvent);

        _domainEvents.Add(CalendarEvents.CreateEventEvent(newTodoEvent));
        return newTodoEvent;
    }

    public void UpdateEvent(TodoEvent todoEvent)
    {
        throw new NotImplementedException();
    }

    public void DeleteEvent(int eventId)
    {
        throw new NotImplementedException();
    }

    public TodoTask GetTask(int taskId)
    {
        var task = _backlog.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
            throw new Exception($"Task {taskId} doesn't exists");
        return task;
    }

    public void BackToBacklog(TodoTask returningTask)
    {
        returningTask.Status = TodoTaskStatus.BackLog;
        // Remove from current day?
        _backlog.Add(returningTask);
    }
    
    public void CompleteTask(TodoTask completedTask)
    {
        completedTask.Status = TodoTaskStatus.Complete;
        _currentDay.Tasks.Remove(completedTask);
        _completed.Add(completedTask);
        SaveChanges();
    }

    public TodoTask AddTask(TodoTask newTask)
    {
        newTask.Id = ++_dbConfig.CurrentIndex;
        if (String.IsNullOrWhiteSpace(newTask.Context))
            newTask.Context = _dbConfig.Context;
        newTask.Created = DateTime.Now;

        _backlog.Add(newTask);
        _domainEvents.Add(TaskEvents.CreateTaskEvent(newTask));
        return newTask;
    }

    public void TackleToday(TodoTask task)
    {
        task.TackleToday(_currentDay.Date);

        _backlog.Remove(task);
        _currentDay!.Tasks.Add(task);
    }

    public void DeleteTask(int taskId)
    {
        var deletingTask = _backlog.Find(t => t.Id == taskId);
        if (deletingTask == null)
            throw new Exception($"Task Entity {taskId} doesn't exists");
        _backlog.Remove(deletingTask);
        _domainEvents.Add(TaskEvents.DeleteTaskEvent(deletingTask));
    }

    public void Update(TodoTask task)
    {
        _domainEvents.AddRange(task.DomainEvents);
        task.ClearDomainEvents();
    }

    public void Delete(TodoTask task)
    {
        throw new NotImplementedException();
    }

    void IFighterTaskContext.AddEvent(TodoEvent todoEvent)
    {
        throw new NotImplementedException();
    }

    public void SaveChanges()
    {
        File.WriteAllText(_dbConfig.ConfigPath, JsonConvert.SerializeObject(_dbConfig, Formatting.Indented));

        File.WriteAllText(_dbConfig.CalendarPath, JsonConvert.SerializeObject(_events, Formatting.Indented));

        LogEvents();

        File.WriteAllText(_dbConfig.BackLogPath, JsonConvert.SerializeObject(_backlog, Formatting.Indented));
        File.WriteAllText(_dbConfig.CompletedPath, JsonConvert.SerializeObject(_completed, Formatting.Indented));
        File.WriteAllText(_dbConfig.TodosPath, JsonConvert.SerializeObject(_todoLists, Formatting.Indented));
    }

    private void UpdateDailyTodoLists()
    {
        var todo = _todoLists.Days.First(d => d.Date == _currentDay.Date);
        todo.Tasks = _currentDay.Tasks;
        todo.Date = _currentDay.Date;
        todo.Opened = _currentDay.Opened;
        todo.ClosedDate = _currentDay.ClosedDate;
    }

    private void LogEvents()
    {
        string content = File.ReadAllText(_dbConfig.LoggerPath);
        List<SerializedEvent> events = JsonConvert.DeserializeObject<List<SerializedEvent>>(content);
        events.AddRange(_domainEvents.Select(d => new SerializedEvent()
        {
            Created = d.Created, Entity = d.Entity, EntityId = d.EntityId, Type = d.Type
        }));
        File.WriteAllText(_dbConfig.LoggerPath, JsonConvert.SerializeObject(events));
    }

    private void SaveChanges(string filePath)
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(_todoLists, Formatting.Indented));
    }

    public void BackUp()
    {
        string formatDate = DateTime.Today.ToString("dd-MM-yyyy");
        SaveChanges(_dbConfig.TodosPath.Split(".json")[0] + formatDate + ".json");
    }

    public void RaiseDomainEvent(BaseEvent<Entity> domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}