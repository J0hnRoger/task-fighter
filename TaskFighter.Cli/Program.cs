using MediatR;
using Spectre.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaskFighter.Domain;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Configuration;
using TaskFighter.Infrastructure.Persistence;

AnsiConsole.MarkupLine("[underline red]Be SOLID[/]");

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