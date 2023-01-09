using System.IO;
using Newtonsoft.Json;
using TaskSamurai.Domain;
using TaskSamurai.Infrastructure.Persistence;

namespace TaskFighter.Tests;

public class TestHelpers
{
    public static SamuraiTasksContext CreateTestContext()
    {
        string configPath = TestConfiguration.GetSecretValue("ConfigPath");

        string configContent = File.ReadAllText(configPath);
        TaskSamuraiConfig config = JsonConvert.DeserializeObject<TaskSamuraiConfig>(configContent);
        
        // CleanUp 
        config.CurrentIndex = 0;
        File.WriteAllText(config.TodosPath,"[]");
        File.WriteAllText(config.RitualsPath,"[]" );
        File.WriteAllText(config.CalendarPath,"[]"  );
        File.WriteAllText(config.LoggerPath,"[]");

        SamuraiTasksContext samuraiTasksContext = new SamuraiTasksContext(config);
        
        return samuraiTasksContext;
    }
}