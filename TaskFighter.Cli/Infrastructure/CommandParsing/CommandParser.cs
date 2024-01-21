using System.Text.RegularExpressions;
using MediatR;

namespace TaskFighter.Infrastructure.CommandParsing;

/// <summary>
/// <entity> <filter> <command> <modifications> <miscellaneous>
/// Je dois donc déduire une commande à partir :
/// d'une Entity (Task/Event/DaySchedule/Log) ET d'une commande (un Verbs)
/// <entity><filter><command><modifications><miscellanous>
/// </summary>
public class CommandParser
{
    private readonly List<Type> _allRequestTypes;
    private IBaseRequest _request;
    private Command _currentCommand;

    // Fix the folowing regex for capture all text between the first group and the third group
    public static readonly string commandParsingRegex = $@"(\w*) (.* )*\b({String.Join("|", VerbType.AllTypes.Select(vt => vt.Name.ToLower()))})\b( .*)*";

    public CommandParser(List<Type> allRequestTypes)
    {
        _allRequestTypes = allRequestTypes;
    }
    
    private void ParseValue(string args)
    {
        _currentCommand.Value = args;
    }

    public IBaseRequest ParseArgs(string commandStr)
    {
        _currentCommand = new Command();

        var result = Regex.Match(commandStr, commandParsingRegex);
        if (!result.Success)
            return new HelpRequest();

        string entityLiteral = result.Groups[1].Value?.Trim();        
        var parsedEntityResult = EntityType.Get(entityLiteral);
        if (parsedEntityResult.IsFailure)
            throw new Exception(parsedEntityResult.Error);
        _currentCommand.Entity = parsedEntityResult.Value;
        
        string filtersLiteral = result.Groups[2].Value?.Trim(); 
        _currentCommand.Filter = new Filters(filtersLiteral);
        
        string verbLiteral = result.Groups[3].Value?.Trim();        
        _currentCommand.Verb = VerbType.Get(verbLiteral);
        
        string valueLiteral = result.Groups[4].Value?.Trim();
        _currentCommand.Value = valueLiteral;

        Type requestType = _allRequestTypes.FirstOrDefault(t => t.Name == _currentCommand.GetRequestName());
        if (requestType == null)
            return new HelpRequest();
        
        IBaseRequest request = (IBaseRequest)Activator.CreateInstance(requestType, _currentCommand.Value, _currentCommand.Filter);
        
        return request;
    }
}