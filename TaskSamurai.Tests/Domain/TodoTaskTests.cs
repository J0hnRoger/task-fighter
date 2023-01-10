using System;
using FluentAssertions;
using TaskSamurai.Domain.TasksManagement;
using TaskSamurai.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class TodoTaskTests
{
    [Fact]
    public void TodoTask_ReturnAge_FromCreated()
    {
        DateTime now = DateTime.Now;
        
        var task = CreateTestTask(now);
        task.GetAge(now).Should().Be("2d");
    }

    [Fact]
    public void TodoTask_ClearEvents_WhenUpdated()
    {
        var testDate = DateTime.Now;
        var todo = CreateTestTask(testDate);
        todo.Start(testDate.AddHours(2));
        todo.DomainEvents.Should().HaveCount(1);
    }
    
    private static TodoTask CreateTestTask(DateTime now)
    {
        TodoTask task = new TodoTask()
        {
            Name = "Test Task",
            Status = TodoTaskStatus.BackLog,
            Created = now.AddDays(-2)
        };
        return task;
    }

}