namespace TaskSamurai.Infrastructure.Persistence;

public class TaskSamuraiConfig
{
    public int CurrentIndex { get; set; }
    public string ConfigPath { get; set; }
    public string TodosPath { get; set; }
    public string CalendarPath { get; set; }
    public string LoggerPath { get; set; }
    
    public string RitualsPath { get; set; }
    public string Context { get; set; }
}