namespace TaskFighter.Infrastructure.CommandParsing;

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
        new VerbType() {Name = "Start"},
        new VerbType() {Name = "Done"},
        new VerbType() {Name = "End"},
        new VerbType() {Name = "Plan"}
    };

    public static VerbType Get(string literal)
    {
        return AllTypes.First(v => v.Name.ToLower() == literal.ToLower());
    }
}