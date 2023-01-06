using TaskSamurai.Domain.Common;
using TaskSamurai.Domain.Common.Interfaces;

namespace TaskSamurai.Domain.TasksManagement;

public class TodoTask : Entity, ITableRenderable
{
    // Creation
    public int Id { get; set; }
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
    public string Status { get; set; }
    
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
            Status ?? "",
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
}