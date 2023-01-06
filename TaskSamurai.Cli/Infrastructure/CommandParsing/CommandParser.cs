using System.Text.RegularExpressions;
using MediatR;
using TaskSamurai.Domain.TasksManagement.Commands;

namespace TaskSamurai.Infrastructure;

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

    public static string commandParsingRegex = @"(\w*) (\S* )*\b(add|modify|delete|list|show)\b( .*)*";

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
            return new NotFoundRequest();

        string entityLiteral = result.Groups[1].Value?.Trim();        
        _currentCommand.Entity = EntityType.Get(entityLiteral);
        
        string filtersLiteral = result.Groups[2].Value?.Trim();        
        _currentCommand.Filter = filtersLiteral;
        
        string verbLiteral = result.Groups[3].Value?.Trim();        
        _currentCommand.Verb = VerbType.Get(verbLiteral);
        
        string valueLiteral = result.Groups[4].Value?.Trim();
        _currentCommand.Value = valueLiteral;

        Type requestType = _allRequestTypes.FirstOrDefault(t => t.Name == _currentCommand.GetRequestName());
        if (requestType == null)
            return new NotFoundRequest();
        
        IBaseRequest request = (IBaseRequest)Activator.CreateInstance(requestType, _currentCommand.Value, _currentCommand.Filter);
        
        return request;
    }
}