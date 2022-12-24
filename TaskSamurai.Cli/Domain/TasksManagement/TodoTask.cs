using TaskSamurai.Domain.Common;

namespace TaskSamurai.Domain.TasksManagement;

public class TodoTask : Entity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public string Context { get; set; }
    public string Parent { get; set; }
    public string Type { get; set; }
    public int Impact { get; set; }
    public DateTime StartDate  { get; set; }
    public DateTime EndDate  { get; set; }
    public string Area { get; set; }
    public string Status { get; set; }
    public List<string> Tags { get; set; }
    public int Difficulty { get; set; }
}