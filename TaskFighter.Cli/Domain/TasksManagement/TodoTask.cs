using System.Reflection;
using TaskFighter.Domain.Common;
using TaskFighter.Domain.Common.Interfaces;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Domain.TasksManagement;

public class TodoTask : Entity, ITableRenderable
{
    public string Name { get; set; }
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
    public bool UnPlanned { get; set; } = false;

    public List<string> Tags { get; set; } = new();

    // Runtime Properties
    public TodoTaskStatus Status { get; set; } = TodoTaskStatus.BackLog;

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

    public string[] GetFieldValues()
    {
        return new[]
        {
            Id.ToString(), Name, Status?.Value ?? TodoTaskStatus.BackLog.Value, string.Join(", ", Tags), Context,
            GetAge(DateTime.Now), Description ?? "", Parent.ToString() ?? "", Type ?? "",
            StartDate.ToString("d") ?? "", EndDate.ToString("d") ?? "", Area ?? "", Impact.ToString() ?? "",
            String.Join(',', Impact) ?? "", Difficulty.ToString()
        };
    }

    /// <summary>
    /// Renvoi la mise uniquement les Property correspondant aux fields
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    public string[] GetFieldValues(List<string> fields)
    {
        var fieldValues = new List<string>();
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        string specialFormat = "[white]{0}[/]";
        if (Status == TodoTaskStatus.Active)
            specialFormat = "[green]{0}[/]";
        else if (Status == TodoTaskStatus.Complete)
            specialFormat = "[grey]{0}[/]";
        
        foreach (string field in fields)
        {
            var property = properties.First(p => p.Name == field);

            var value = property.GetValue(this);
            string stringValue;
            if (value is IEnumerable<string> stringEnumerable)
            {
                stringValue = string.Join(", ", stringEnumerable);
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                stringValue = ((DateTime?)value)?.ToString("d") ?? "";
            }
            else
            {
                stringValue = string.Format(specialFormat, value?.ToString() ?? "" );
            }

            fieldValues.Add(stringValue);
        }

        return fieldValues.ToArray();
    }

    public void Finish(DateTime endDate, bool force = false)
    {
        if (!force && Status != TodoTaskStatus.Active)
            throw new Exception($"Asked task {Id} not active");
        Status = TodoTaskStatus.Complete;
        EndDate = endDate;

        _domainEvents.Add(new TaskCompletedEvent(this, endDate));
    }

    public void Start(DateTime startDate)
    {
        if (Status != TodoTaskStatus.Planned)
            throw new Exception($"Asked task {Id} not in the Todo today");
        Status = TodoTaskStatus.Active;
        StartDate = startDate;

        _domainEvents.Add(new TaskStartedEvent(this, startDate));
    }

    public override string ToString()
    {
        return $"{Id} - {Name} - {Status}";
    }

    public void Tackle(DateTime today)
    {
        Status = TodoTaskStatus.Planned;
        // _domainEvents.Add(new TaskPlannedEvent(this, today));
    }

    public static TodoTask CreateFromBatchLine(string batchLine, string project)
    {
        string[] properties = batchLine.Split(";");
        return new TodoTask() {Name = properties[0],};
    }
}