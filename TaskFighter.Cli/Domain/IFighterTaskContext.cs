﻿using CSharpFunctionalExtensions;
using TaskFighter.Domain.Common;
using TaskFighter.Domain.EventsManagement;
using TaskFighter.Domain.TasksManagement;
using Entity = TaskFighter.Domain.Common.Entity;

namespace TaskFighter.Domain;

public interface IFighterTaskContext
{
    public IReadOnlyList<TodoTask> Tasks { get; }
    public IReadOnlyList<TodoEvent> Events { get; }
    public IReadOnlyList<TodoTask> Backlog { get; }
    public DailyTodoLists DailyTodoLists { get; } 
    public DailyTodo DailyTodo { get; }
    public DailyTodo Tomorrow { get; }
    string Context { get;}

    Result<TodoTask> GetTask(int requestTaskId);
    public TodoTask AddTask(TodoTask newTask);
    void DeleteTask(int taskId);
    public void Update(TodoTask task);
    public void Delete(TodoTask task);

    public void AddToTodoList(TodoTask task, DailyTodo todoList);
    public void BackToBacklog(TodoTask returningTask);
    public void CompleteTask(TodoTask completedTask);
    public void Migrate(TodoTask migrateTask);
    
    void AddEvent(TodoEvent todoEvent);
    void UpdateEvent(TodoEvent todoEvent);
    void DeleteEvent(int eventId);
    void DeleteFromDailyTodo(TodoTask task, int todoListId);
    void SetContext(string context);
    public void SaveChanges();
    public void BackUp();
    void RaiseDomainEvent(BaseEvent<Entity> domainEvent);
    void AddTask(TodoTask newTask, DailyTodo todoList);
}