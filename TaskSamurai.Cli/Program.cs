using MediatR;
using Spectre.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaskSamurai.Domain;
using TaskSamurai.Domain.DayScheduling;
using TaskSamurai.Domain.DayScheduling.Requests;
using TaskSamurai.Domain.TasksManagement;
using TaskSamurai.Domain.TasksManagement.Commands;
using TaskSamurai.Infrastructure;
using TaskSamurai.Infrastructure.Persistence;
using TaskSamurai.Infrastructure.Renderer;

AnsiConsole.MarkupLine("[underline red]It's a good day to be the GOAT[/]");
AnsiConsole.MarkupLine("[underline red]Be SOLID![/]");

IConfigurationRoot configuration = GetConfiguration();
var builder = ConfigureServices();

ServiceProvider ConfigureServices()
{
    string configPath = configuration["ConfigPath"];
    string configContent = File.ReadAllText(configPath);
    TaskSamuraiConfig config = JsonConvert.DeserializeObject<TaskSamuraiConfig>(configContent);
    
    var allDomainRequestTypes = typeof(CommandParser)
        .Assembly
        .GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IBaseRequest)))
        .ToList();
    
    return new ServiceCollection()
        .AddSingleton<IConfigurationRoot>(p => configuration)
        .AddSingleton(p =>  new CommandParser(allDomainRequestTypes))
        .AddSingleton<ISamuraiTaskContext>(prodiver => new SamuraiTasksContext(config))
        .AddMediatR(typeof(ShowConfigRequestHandler ))
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
    TaskFighterTable<TodoTask> table = new TaskFighterTable<TodoTask>(result);
    AnsiConsole.Write(table.Table);
}

if (command is GenerateDayScheduleRequest)
{
    var result = await _mediator.Send(new GenerateDayScheduleRequest(){ CurrentDay = DateTime.Now });
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

var commandResult = await _mediator.Send(command);
AnsiConsole.WriteLine($"{command} - {commandResult}");

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