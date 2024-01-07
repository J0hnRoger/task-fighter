using TaskFighter.Domain.Common;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Domain.RitualsManagement;

/// <summary>
/// Rituals generate recurring tasks at different frequency
/// </summary>
public class Ritual : Entity
{
   public string Name { get; set; }
   public Frequency Frequency { get; set; }
   public int TimeByFrequency { get; set; } = 1;
   public int Duration { get; set; } 
   public DateTime StartDate { get; set; }
   public DateTime EndDate { get; set; }
   public string Area { get; set; }
   
   public DateTime LastExecution { get; set; } 
   public TodoTask TaskTemplate { get; set; }

   public TodoTask GenerateTask()
   {
       // Frequency Check
       return TaskTemplate;
   } 
}