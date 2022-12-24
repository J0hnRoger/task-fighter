namespace TaskSamurai.Domain.Common.Interfaces;

public interface ISamuraiTaskEvent 
{
    public string Type { get; }
    public DateTime Created { get; }
}