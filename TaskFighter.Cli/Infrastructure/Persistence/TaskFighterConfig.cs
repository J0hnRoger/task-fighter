﻿namespace TaskFighter.Infrastructure.Persistence;

public class TaskFighterConfig
{
    public int CurrentTaskIndex { get; set; }
    public int CurrentTodoListIndex { get; set; }
    public string ConfigPath { get; set; }
    /// <summary>
    /// Fichier contenant les tâches à réaliser dans la journée/semaine
    /// </summary>
    public string TodosPath { get; set; }
    /// <summary>
    /// Fichier contenant toutes les tâches à réaliser (backlog)
    /// </summary>
    public string BackLogPath { get; set; }
    public string CalendarPath { get; set; }
    public string LoggerPath { get; set; }
    public string Context { get; set; }
    
    public List<string> AllContexts { get; set; }

    public void InitPaths()
    {
        BackLogPath = BackLogPath.Replace("{context}", Context);
        TodosPath = TodosPath.Replace("{context}", Context);
        CalendarPath = CalendarPath.Replace("{context}", Context);
    }
    
    public string GetBasePath()
    {
        int lastSlash = ConfigPath.LastIndexOf("/", StringComparison.Ordinal);
        return ConfigPath.Substring(0, lastSlash);
    }
    
    public string GetWorkingDirectory()
    {
        return Path.Combine(GetBasePath(), Context);
    }
}