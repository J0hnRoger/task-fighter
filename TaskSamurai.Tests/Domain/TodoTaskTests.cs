using System;
using FluentAssertions;
using TaskSamurai.Domain.TasksManagement;
using Xunit;

namespace TaskFighter.Tests;

public class TodoTaskTests
{
    [Fact]
    public void TodoTask_ReturnAge_FromCreated()
    {
        DateTime now = DateTime.Now;
        
        TodoTask task = new TodoTask()
        {
           Created = now.AddDays(-2) 
        };
        task.GetAge(now).Should().Be("2d");
    }
}