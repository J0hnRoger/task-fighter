using System.Reflection;
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
    private readonly DailyTodo _tomorrow;

    private DailyTodoLists _todoLists { get; set; }
    private List<TodoTask> _backlog { get; set; }
    private List<TodoEvent> _events { get; set; }

    public IReadOnlyList<TodoTask> Backlog => _backlog;

    public DailyTodoLists DailyTodoLists => _todoLists;
    public IReadOnlyList<TodoTask> Tasks => _currentDay.Tasks;
    public DailyTodo DailyTodo => _currentDay;
    public DailyTodo Tomorrow => _tomorrow;

    public IReadOnlyList<TodoEvent> Events => _events;

    public string Context => _dbConfig.Context;

    /// <summary>
    /// Unit of Work for recording tasks
    /// </summary>
    /// <param name="dbConfig">Config path, for index</param>
    public FighterTasksContext(TaskFighterConfig dbConfig)
    {
        _dbConfig = dbConfig;
        _dbConfig.InitPaths(); 
        
        string context = _dbConfig.Context; 
        string backlogPath = Load(_dbConfig.BackLogPath);
        string dailyTodoPath = Load(_dbConfig.TodosPath);

        _backlog = JsonConvert.DeserializeObject<List<TodoTask>>(backlogPath
            , new TodoTaskStatusJsonConverter(typeof(TodoTaskStatus))) ?? new List<TodoTask>();

        _todoLists = JsonConvert.DeserializeObject<DailyTodoLists>(dailyTodoPath,
                         new TodoTaskStatusJsonConverter(typeof(TodoTaskStatus)))
                     ?? new DailyTodoLists();

        var today = DateTime.Today;


        _currentDay = GetOrCreateTodoList(today);
        ;
        _tomorrow = GetOrCreateTodoList(today.AddDays(1));

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
        context = context.ToLower(); 
        string updatingTodoPath = _dbConfig.TodosPath.Replace($"/{_dbConfig.Context}/", $"/{context}/");
        string updatingBacklogPath = _dbConfig.BackLogPath.Replace($"/{_dbConfig.Context}/", $"/{context}/");
            
        if (!_dbConfig.AllContexts.Contains(context))
        {
            _dbConfig.AllContexts.Add(context);
            if (!File.Exists(updatingTodoPath))
            {
                string? directoryPath = Path.GetDirectoryName(updatingTodoPath);
                if (directoryPath != null && !Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                
                string assemblyPath = Assembly.GetExecutingAssembly().Location;
                string assemblyDirectory = Path.GetDirectoryName(assemblyPath)!;
                
                string backlogContent = File.ReadAllText($"{assemblyDirectory }/assets/backlog-template.json"); 
                string tasksContent = File.ReadAllText($"{assemblyDirectory}/assets/todo-template.json"); 
                
                File.WriteAllText(updatingTodoPath, tasksContent);
                File.WriteAllText(updatingBacklogPath, backlogContent);
            }
        }

        _dbConfig.BackLogPath = updatingBacklogPath; 
        _dbConfig.TodosPath = updatingTodoPath; 
        _dbConfig.Context = context;
            
        // We only save the config file for not copying current todo and backlog in the new context 
        File.WriteAllText(_dbConfig.ConfigPath, JsonConvert.SerializeObject(_dbConfig, Formatting.Indented));
    }

    public void Migrate(TodoTask migrateTask)
    {
        var tomorrow = DateTime.Today.AddDays(1);
        var dailyTodo = GetOrCreateTodoList(tomorrow);
        
        if (migrateTask.Status == TodoTaskStatus.Planned)
            _currentDay.Tasks.Remove(migrateTask);
        
        dailyTodo.Tasks.Add(migrateTask);
        SaveChanges();
    }

    private DailyTodo GetOrCreateTodoList(DateTime day)
    {
        var dailyTodo = _todoLists.GetTodoList(day);
        if (dailyTodo == null)
        {
            _dbConfig.CurrentTodoListIndex++; 
            dailyTodo = new DailyTodo(_dbConfig.CurrentTodoListIndex)
            {
                Date = day, Tasks = new List<TodoTask>(), Opened = false
            };
            _todoLists.Days.Add(dailyTodo);
            SaveChanges();
        }

        return dailyTodo;
    }

    public TodoEvent AddEvent(TodoEvent newTodoEvent)
    {
        newTodoEvent.Id = ++_dbConfig.CurrentTaskIndex;
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
        SaveChanges();
    }
    
    public void CompleteTask(TodoTask completedTask)
    {
        completedTask.Status = TodoTaskStatus.Complete;
        SaveChanges();
    }

    private TodoTask CreateTask(TodoTask newTask)
    {
        newTask.Id = ++_dbConfig.CurrentTaskIndex;
        newTask.Context = Context;
        if (String.IsNullOrWhiteSpace(newTask.Context))
            newTask.Context = _dbConfig.Context;
        newTask.Created = DateTime.Now;
        return newTask;
    }

    public void AddTask(TodoTask newTask, DailyTodo todoList)
    {
        var task = CreateTask(newTask);
        todoList.Tasks.Add(task);
        _domainEvents.Add(TaskEvents.CreateTaskEvent(newTask));
    }
    
    public TodoTask AddTask(TodoTask newTask)
    {
        newTask.Id = ++_dbConfig.CurrentTaskIndex;
        if (String.IsNullOrWhiteSpace(newTask.Context))
            newTask.Context = _dbConfig.Context;
        newTask.Created = DateTime.Now;
        _backlog.Add(newTask);
        _domainEvents.Add(TaskEvents.CreateTaskEvent(newTask));
        return newTask;
    }

    public void AddToTodoList(TodoTask task, DailyTodo todoList)
    {
        task.Tackle(todoList.Date);

        _backlog.Remove(task);
        todoList.Tasks.Add(task);
    }

    public void DeleteTask(int taskId)
    {
        var deletingTask = _backlog.Find(t => t.Id == taskId);

        if (deletingTask != null)
        {
            _backlog.Remove(deletingTask);
            return;
            // _domainEvents.Add(TaskEvents.DeleteTaskEvent(deletingTask));
        }

        deletingTask = _currentDay.Tasks.Find(t => t.Id == taskId);
        if (deletingTask != null)
        {
            _currentDay.Tasks.Remove(deletingTask);
            return;
        }

        throw new Exception($"Task Entity {taskId} doesn't exists in Backlog or DailyTodo");
    }

    public void Update(TodoTask task)
    {
        _domainEvents.AddRange(task.DomainEvents);
        task.ClearDomainEvents();
    }

    public void DeleteFromDailyTodo(TodoTask task, int todoId)
    {
        var todoList = DailyTodoLists.GetTodoList(todoId);
        if (todoList == null)
            throw new Exception("TodoList doesn't exists");

        var deleting = todoList.Tasks.FirstOrDefault(t => t.Id == task.Id);
        if (deleting == null)
            throw new Exception("Task doesn't exists in DailyTodo");
        _currentDay.Tasks.Remove(deleting);

        _domainEvents.Add(TaskEvents.DeleteTaskEvent(deleting));
        SaveChanges();
    }

    public void Delete(TodoTask task)
    {
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