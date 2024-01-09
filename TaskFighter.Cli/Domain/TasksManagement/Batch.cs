namespace TaskFighter.Domain.TasksManagement;

public class Batch
{
    public const string PROJECT_HEADER_MARK = "#";
    public const string COMMENT_MARK = "//";
    
    public List<TodoTask> Tasks { get; set; }
    
    public Batch(string batchContent)
    {
        string[] taskLines = batchContent.Split('\n');
        Dictionary<string, List<TodoTask>> projectTasks = new();
        string currentProject = "";
        
        foreach (string taskLine in taskLines)
        {
            if (taskLine.StartsWith(PROJECT_HEADER_MARK))
            {
                currentProject = taskLine.TrimStart('#').TrimEnd().TrimEnd('\r').Split(";")[0]; 
                projectTasks.Add(currentProject, new List<TodoTask>());
                continue;
            }
            
            if (taskLine.Trim().Length == 0)
                continue;
            
            if (taskLine.ToCharArray().Length > 0 
                && int.TryParse(taskLine[0].ToString(), out int taskId))
            {
                // Already created
                continue;
            }
            
            if (taskLine.StartsWith(COMMENT_MARK))
                continue;
            
            projectTasks[currentProject].Add(TodoTask.CreateFromBatchLine(taskLine.TrimEnd('\n').TrimEnd('\n').Trim(), currentProject));
        }

        Tasks = projectTasks.SelectMany(t => t.Value).ToList();
    }
}