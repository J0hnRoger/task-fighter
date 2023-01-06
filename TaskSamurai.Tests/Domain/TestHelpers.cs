using TaskSamurai.Domain;
using TaskSamurai.Infrastructure.Persistence;

namespace TaskFighter.Tests;

public class TestHelpers
{
    public static SamuraiTasksContext CreateTestContext()
    {
        string configDir = TestConfiguration.GetSecretValue("ConfigPath");
        
        TaskSamuraiConfig config = new TaskSamuraiConfig()
        {
           CurrentIndex = 0,
           ConfigPath = configDir + "test.config.json",
           TodosPath =  configDir + "test.tasks.json", 
           RitualsPath =  configDir + "test.rituals.json", 
           CalendarPath =  configDir + "test.calendar.json", 
           LoggerPath = configDir + "test.database.json",
           Context = "test"
        };
        
        SamuraiTasksContext samuraiTasksContext = new SamuraiTasksContext(config);
        return samuraiTasksContext;
    }
}