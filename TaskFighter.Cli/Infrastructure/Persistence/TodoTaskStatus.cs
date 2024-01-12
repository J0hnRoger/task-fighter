using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace TaskFighter.Infrastructure.Persistence;

public class TodoTaskStatus : ValueObject
{
    public string Value { get; set; }
    
    public static TodoTaskStatus BackLog = new TodoTaskStatus() { Value = "Backlog" };
    public static TodoTaskStatus Planned = new TodoTaskStatus() { Value = "Planned" };
    public static TodoTaskStatus Active = new TodoTaskStatus() { Value = "Active" };
    public static TodoTaskStatus Complete = new TodoTaskStatus() { Value = "Complete" };
    
    // TODO - state machine
    protected override IEnumerable<object> GetEqualityComponents()
    {
       yield return  Value;
    }

    public override string ToString()
    {
        return Value;
    }
}