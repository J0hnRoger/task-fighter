using System.IO;
using Newtonsoft.Json;
using TaskFighter.Infrastructure.Persistence;

namespace TaskFighter.Tests;

public class TestHelpers
{
    public static FighterTasksContext CreateTestContext()
    {
        string configPath = TestConfiguration.GetSecretValue("ConfigPath");

        string configContent = File.ReadAllText(configPath);
        TaskFighterConfig config = JsonConvert.DeserializeObject<TaskFighterConfig>(configContent);
        
        // CleanUp 
        config.CurrentIndex = 0;
        File.WriteAllText(config.TodosPath,"[]");
        File.WriteAllText(config.BackLogPath,"[]" );
        File.WriteAllText(config.CalendarPath,"[]"  );
        File.WriteAllText(config.LoggerPath,"[]");

        FighterTasksContext fighterTasksContext = new FighterTasksContext(config);
        
        return fighterTasksContext;
    }
}