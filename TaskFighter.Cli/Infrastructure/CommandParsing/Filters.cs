using System.Text.RegularExpressions;

namespace TaskFighter.Infrastructure.CommandParsing;

public class Filters
{
    public string Raw { get; }
    public List<string> SpecialTags { get; } = new();
    public List<string> Tags { get; set; } = new();
    
    // public static string tagsRegex = $@"(?<tags>(\+[\w]+)*)";
    public static string tagsRegex = $@"(?<tags>\+([\w]+)*)";

    public Filters(string filters)
    {
        Raw = filters;
        var tagResult = Regex.Matches(filters, tagsRegex);

        foreach (Match resultGroup in tagResult)
        {
            var tag = resultGroup.Groups[1].Value.Trim();
            if (Domain.TasksManagement.Tags.SpecialTags.Contains(tag))
                SpecialTags.Add(tag);
            else 
                Tags.Add(tag);
        }

        Tags = Tags.Distinct().ToList();
    }
}