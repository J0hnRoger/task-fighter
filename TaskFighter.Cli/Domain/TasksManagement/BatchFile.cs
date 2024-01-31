using System.Text;

namespace TaskFighter.Domain.TasksManagement;

public class BatchFile
{
    public const string TAG_MARK = "#";
    public const string COMMENT_MARK = "//";
    public const string TASK_MARK = "-";
    public const string ANNOTATION_MARK = "--";

    public string Content { get; set; }
    public List<TodoTask> Tasks { get; set; }

    public BatchFile(List<TodoTask> tasks)
    {
        Tasks = tasks;
        Content = GenerateMarkdown();
    }

    private string GenerateMarkdown()
    {
        StringBuilder content = new();
        foreach (var task in Tasks)
            FormatTask(content, task);
        return content.ToString();
    }

    private StringBuilder FormatTask(StringBuilder content, TodoTask task)
    {
        content.AppendLine($"- {task.Id} {task.Name} - {string.Join(",", task.Tags.Select(t => $"#{t}"))}");
        return content;
    }

    public override string ToString()
    {
        return Content;
    }

    public BatchFile(string batchContent)
    {
        Content = batchContent;
        ParseContent();
    }

    private List<TodoTask> ParseContent()
    {
        string[] taskLines = Content.Split('\n')
            .Where(t => !String.IsNullOrWhiteSpace(t))
            .ToArray();

        List<TodoTask> tasks = new List<TodoTask>();

        foreach (string taskLine in taskLines)
        {
            if (!taskLine.Contains(TASK_MARK))
                continue;
           
            string taskContent = taskLine.Split("- ")[1];
            int.TryParse(taskContent.Split(" ")[0], out int taskId);

            var descriptionAndTags = taskLine.Substring("- ".Length + taskId.ToString().Length)
                .Trim();
            var tagsContent = descriptionAndTags.Split(" - ")[1];
            var description = descriptionAndTags.Split(" - ")[0];
            var tags = tagsContent.Split(new[] {TAG_MARK}, StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => tag.Trim()).ToList();

            tasks.Add(new TodoTask() {Id = taskId, Name = description, Tags = tags});
        }

        return tasks;
    }
}