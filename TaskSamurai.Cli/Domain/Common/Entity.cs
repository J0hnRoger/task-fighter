namespace TaskSamurai.Domain.Common;

public abstract class Entity
{
      private List<BaseEvent<Entity>> _domainEvents = new List<BaseEvent<Entity>>();
      public int Id { get; set; }
}