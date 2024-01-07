namespace TaskFighter.Infrastructure.Persistence;

public class SerializedEvent 
{
    public int EntityId { get; set; }
    public string Type { get; set; }
    public DateTime Created { get; set; }
    public object Entity { get; set; }
}