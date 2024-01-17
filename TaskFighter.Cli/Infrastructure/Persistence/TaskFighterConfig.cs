namespace TaskFighter.Infrastructure.Persistence;

public class TaskFighterConfig
{
    public int CurrentIndex { get; set; }
    public string ConfigPath { get; set; }
    /// <summary>
    /// Fichier contenant les tâches à réaliser dans la journée/semaine
    /// </summary>
    public string TodosPath { get; set; }
    /// <summary>
    /// Fichier contenant toutes les tâches à réaliser (backlog)
    /// </summary>
    public string BackLogPath { get; set; }
    
    /// <summary>
    /// Fichier contenant toutes les tâches terminées - pour du reporting
    /// </summary>
    public string CompletedPath { get; set; }
    public string CalendarPath { get; set; }
    public string LoggerPath { get; set; }
    public string Context { get; set; }

    public string GetBasePath()
    {
        int lastSlash = ConfigPath.LastIndexOf("/", StringComparison.Ordinal);
        return ConfigPath.Substring(0, lastSlash);
    }
}