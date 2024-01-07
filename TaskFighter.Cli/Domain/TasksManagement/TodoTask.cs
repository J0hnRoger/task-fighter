using TaskFighter.Domain.Common;
using TaskFighter.Domain.Common.Interfaces;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement;

public class TodoTask : Entity, ITableRenderable
{
    // Creation
    public string Name { get; set; }
    
    public string Project { get; set; }
    public string Context { get; set; }
    public string Description { get; set; }
    public int Parent { get; set; }
    public string Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime EndDate { get; set; }
    public int EstimateDuration { get; set; }
    public string Area { get; set; }
    public int Impact { get; set; }
    public List<string> Tags { get; set; }
    
    // Runtime Properties
    public TodoTaskStatus Status { get; set; }
    
    // Reporting Properties
    public int Difficulty { get; set; }

    public string GetAge(DateTime current)
    {
        TimeSpan elapsedTime = current.Subtract(Created);
        if (elapsedTime.TotalDays > 1)
            return elapsedTime.Days.ToString() + "d";
        
        if (elapsedTime.TotalHours > 1)
            return elapsedTime.Hours.ToString() + "h";
        
        if (elapsedTime.TotalMinutes > 1)
            return elapsedTime.Minutes.ToString() + "m";
        return elapsedTime.Seconds.ToString() + "s";
    }

    public string[] GetFields()
    {
        return new[]
        {
            Id.ToString(),
            Name,
            Status?.Value ?? TodoTaskStatus.BackLog.Value,
            Context,
            GetAge(DateTime.Now),
            Description ?? "",
            Parent.ToString() ?? "",
            Type ?? "",
            StartDate.ToString("d") ?? "",
            EndDate.ToString("d") ?? "",
            Area ?? "",
            Impact.ToString() ?? "",
            String.Join(',', Impact) ?? "",
            Difficulty.ToString()
        };
    }

    public void Finish(DateTime endDate)
    {
        if (Status != TodoTaskStatus.Active)
            throw new Exception($"Asked task {Id} not active");
        Status = TodoTaskStatus.Complete;
        EndDate = endDate;
        
        _domainEvents.Add(new TaskCompletedEvent(this, endDate));
    }

    public void Start(DateTime startDate)
    {
        if (Status != TodoTaskStatus.BackLog)
            throw new Exception($"Asked task {Id} not in backlog");
        Status = TodoTaskStatus.Active;
        StartDate = startDate;
        
        _domainEvents.Add(new TaskStartedEvent(this, startDate));
    }

    public override string ToString()
    {
        return $"{Id} - {Name} - {Status}";
    }
}