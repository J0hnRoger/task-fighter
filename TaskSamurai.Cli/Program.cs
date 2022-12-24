using MediatR;
using Spectre.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskSamurai.Domain.DayScheduling;
using TaskSamurai.Domain.DayScheduling.Commands;
using TaskSamurai.Infrastructure;
using TaskSamurai.Infrastructure.Renderer;

AnsiConsole.MarkupLine("[underline red]It's a good day to be the GOAT[/]");
AnsiConsole.MarkupLine("[underline red]Be SOLID![/]");

IConfigurationRoot configuration = GetConfiguration();
var builder = ConfigureServices();

ServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddSingleton<CommandParser>()
        .AddMediatR(typeof(GenerateDayScheduleCommandHandler))
        .BuildServiceProvider();
}

CommandParser commandParser = builder.GetRequiredService<CommandParser>();

var command = commandParser.ParseArgs(Environment.GetCommandLineArgs());

ISender _mediator =  builder.GetService<ISender>();

if (command is GenerateDayScheduleCommand)
{
    var result = await _mediator.Send(new GenerateDayScheduleCommand(){ CurrentDay = DateTime.Now });
    // Persist day on the DB.json
    TaskFighterTable<TimeBlock> table = new TaskFighterTable<TimeBlock>(result.TimeBlocks.ToList());
    AnsiConsole.Render(table.Table);
    if (!AnsiConsole.Confirm("On envoit la poudre?"))
    {
        result.StartDay(DateTime.Now);
    }
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

DaySchedule dayWork = new DaySchedule();
dayWork.TimeBlocks.Add(new TimeBlock()
{
    Interval = new TimeInterval()
    {
        StartTime = new Time(DateTime.Now.Hour, DateTime.Now.Minute - 1),
        EndTime = new Time(DateTime.Now.Hour, DateTime.Now.Minute + 1),
    }
});
dayWork.OnTimeBlockEnd += block => AnsiConsole.WriteLine(block.Interval.ToString() + " finished");
// Craft your day
dayWork.StartDay(DateTime.Now);
Console.ReadLine();