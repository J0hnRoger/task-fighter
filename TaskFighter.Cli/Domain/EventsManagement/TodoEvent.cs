using TaskFighter.Domain.Common;
using TaskFighter.Domain.TasksManagement;

namespace TaskFighter.Domain.EventsManagement;

public class TodoEvent : Entity
{
   public string Name { get; set; }
   public string Description { get; set; }
   public DateTime StartDate { get; set; }  
   public DateTime EndDate { get; set; }
   public List<TodoTask> Requirement { get; set; } = new List<TodoTask>();
   public bool HasRequirements => Requirement.Count > 0;
   
   public string AreaOfCompetence { get; set; }
}