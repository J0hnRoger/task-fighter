namespace TaskSamurai.Infrastructure;

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
    public static EntityType DaySchedule = new EntityType("DaySchedule", "day");
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