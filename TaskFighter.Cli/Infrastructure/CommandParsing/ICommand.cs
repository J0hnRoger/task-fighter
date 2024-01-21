namespace TaskFighter.Infrastructure.CommandParsing;

// Builder pattern
public class Command
{
    public EntityType Entity { get; set; }
    public VerbType Verb { get; set; }
    public Filters Filter { get; set; }
    public string Value { get; set; }

    public string GetRequestName()
    {
        return  Verb.Name + Entity.Name + "Request";
    }
}