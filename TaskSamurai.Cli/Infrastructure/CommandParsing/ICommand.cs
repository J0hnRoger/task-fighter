using FluentAssertions.Types;
using MediatR;
using TaskSamurai.Domain.TasksManagement.Commands;

namespace TaskSamurai.Infrastructure;

public class CreateTaskCommand : Command
{
    private IBaseRequest _request;
        
}

// Builder pattern
public class Command
{
    public EntityType Entity { get; set; }
    public VerbType Verb { get; set; }
    public string Filter { get; set; }
    public string Value { get; set; }

    public string GetRequestName()
    {
        return  Verb.Name + Entity.Name + "Request";
    }
}

public class VerbType
{
    public string Name { get; set; }

    public static List<VerbType> AllTypes = new List<VerbType>()
    {
        new VerbType() {Name = "Add"},
        new VerbType() {Name = "Modify"},
        new VerbType() {Name = "Delete"},
        new VerbType() {Name = "List"},
        new VerbType() {Name = "Show"},
    };

    public static VerbType Get(string literal)
    {
        return AllTypes.First(v => v.Name.ToLower() == literal.ToLower());
    }
}

public class EntityType
{
    public string Name { get; set; }
    public string ShortCut { get; set; }

    public EntityType(string name, string shortCut)
    {
        ShortCut = shortCut;
        Name = name;
    }
    
    public static EntityType TaskType = new EntityType("Task", "t");
    public static EntityType EventType = new EntityType("Event", "e");
    public static EntityType WorkflowType = new EntityType("Workflow", "w");
    public static EntityType TimeBlock = new EntityType("TimeBlock", "tb");
    public static EntityType DaySchedule = new EntityType("DaySchedule", "ds");
    public static EntityType ConfigType = new EntityType("Config", "config");
    
    public static List<EntityType> AllTypes = new List<EntityType>()
    {
        TaskType, EventType, WorkflowType, TimeBlock, DaySchedule, ConfigType
    };

    public static EntityType Get(string literal)
    {
        return AllTypes.First(e => e.Name.ToLower() == literal.ToLower()
        || e.ShortCut.ToLower() == literal.ToLower());
    }
}

public abstract class BaseCommand
{
    public IRequest CommandType;
    public abstract bool TryParse(string arg, out string rest);

    public virtual IBaseRequest GetCommand()
    {
        return new AddTaskRequest("", "");
    }
}