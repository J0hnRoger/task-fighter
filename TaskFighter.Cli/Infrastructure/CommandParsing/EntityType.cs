using CSharpFunctionalExtensions;

namespace TaskFighter.Infrastructure.CommandParsing;

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
    public static EntityType DaySchedule = new EntityType("Day", "day");
    public static EntityType ConfigType = new EntityType("Config", "config");
    public static EntityType ReportType = new EntityType("Report", "report");
    public static EntityType ContextType = new EntityType("Context", "context");
    
    public static List<EntityType> AllTypes = new List<EntityType>()
    {
        TaskType, EventType, DaySchedule, ConfigType, ReportType, ContextType
    };

    public static Result<EntityType> Get(string literal)
    {
        var entity = AllTypes.FirstOrDefault(e => e.Name.ToLower() == literal.ToLower()
                                   || e.ShortCut.ToLower() == literal.ToLower());
        if (entity == null)
            return Result.Failure<EntityType>($"Entity {literal} not found");
        return entity;
    }
}