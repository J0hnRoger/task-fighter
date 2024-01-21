using System.Text.RegularExpressions;

namespace TaskFighter.Infrastructure.CommandParsing;

public class Filters
{
    public string Raw { get; }
    public List<string> Tags { get; set; } = new();
    public Dictionary<string, string> PropertiesFilter { get; set; } = new();
    
    // public static string tagsRegex = $@"(?<tags>(\+[\w]+)*)";
    public static string tagsRegex = $@"(?<tags>\+([\w]+)*)";

    public Filters(string filters)
    {
        Raw = filters;
        var tagResult = Regex.Matches(filters, tagsRegex);

        foreach (Match resultGroup in tagResult)
        {
            Tags.Add(resultGroup.Groups[1].Value.Trim());
        }

        Tags = Tags.Distinct().ToList();
    }
}