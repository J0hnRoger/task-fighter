namespace TaskFighter.Domain.Common.Interfaces;

public interface ITodoTaskEvent 
{
    public string Type { get; }
    public DateTime Created { get; }
}