using MediatR;
using Spectre.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaskFighter.Domain;
using TaskFighter.Domain.DayScheduling;
using TaskFighter.Domain.DayScheduling.Requests;
using TaskFighter.Domain.TasksManagement;
using TaskFighter.Domain.TasksManagement.Requests;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Configuration;
using TaskFighter.Infrastructure.Persistence;
using TaskFighter.Infrastructure.Renderer;

AnsiConsole.MarkupLine("[underline red]It's a good day to be the GOAT[/]");
AnsiConsole.MarkupLine("[underline red]Be SOLID![/]");

IConfigurationRoot configuration = GetConfiguration();
var builder = ConfigureServices();

ServiceProvider ConfigureServices()
{
    string configPath = configuration["ConfigPath"];
    string configContent = File.ReadAllText(configPath);
    TaskFighterConfig config = JsonConvert.DeserializeObject<TaskFighterConfig>(configContent);
    
    var allDomainRequestTypes = typeof(CommandParser)
        .Assembly
        .GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IBaseRequest)))
        .ToList();
    
    
    return new ServiceCollection()
        .AddSingleton<IConfigurationRoot>(p => configuration)
        .AddSingleton<TaskFighterConfig>(p => config)
        .AddSingleton(p =>  new CommandParser(allDomainRequestTypes))
        .AddSingleton<IFighterTaskContext>(prodiver => new FighterTasksContext(config))
        .AddMediatR(typeof(ShowConfigRequestHandler))
        .BuildServiceProvider();
}

CommandParser commandParser = builder.GetRequiredService<CommandParser>();

string commandStr = String.Join(" ", args);
var command = commandParser.ParseArgs(commandStr);

ISender _mediator =  builder.GetService<ISender>();

if (command is NotFoundRequest)
{
   AnsiConsole.MarkupLine($"[red bold]Commande inconnue: {commandStr}[/]");
   return;
}

if (command is ListTaskRequest listTasksCommand)
{
    var result = await _mediator.Send(listTasksCommand);
   
    if (result.Project == "backlog")
        AnsiConsole.MarkupLine($"[bold]Backlog[/]");
    
    else if (result.Day.HasValue)
    {
        AnsiConsole.MarkupLine($"[bold]Daily Todo: {result.Day.ToString()}[/]");
    }
    
    TaskFighterTable<TodoTask> table = new(result.Tasks, new List<string>()
    {
       "Id", "Name", "Status", "Project"  
    });
    AnsiConsole.Write(table.Table);
    return;
}

if (command is PlanDayScheduleRequest planDayRequest)
{
    var result = await _mediator.Send(planDayRequest);
    // Persist day on the DB.json
    TaskFighterTable<TimeBlock> table = new TaskFighterTable<TimeBlock>(result.TimeBlocks.ToList());
    foreach (var tb in result.TimeBlocks)
    {
        AnsiConsole.WriteLine(tb.Interval.ToString());
    }
    
    if (!AnsiConsole.Confirm("Fais parler la poudre?"))
    {
        result.StartDay(DateTime.Now);
    }
}

try
{
    var commandResult = await _mediator.Send(command);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}


/// <summary>
/// Récupération des données de configuration
/// </summary>
static IConfigurationRoot GetConfiguration()
{
    // Charge la configuration 
    return new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();
}