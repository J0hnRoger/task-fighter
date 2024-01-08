using MediatR;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Json;

namespace TaskFighter.Infrastructure.Configuration;

public record ShowConfigRequest : IRequest<Unit>
{
    public ShowConfigRequest(string value, string filters) { }

    public override string ToString()
    {
        return "Config:";
    }
}

public class ShowConfigRequestHandler : IRequestHandler<ShowConfigRequest>
{
    private readonly string _configPath;

    public ShowConfigRequestHandler(IConfigurationRoot configPath)
    {
        _configPath = configPath["ConfigPath"];
    }
    
   public Task<Unit> Handle(ShowConfigRequest request, CancellationToken cancellationToken)
   {
       AnsiConsole.WriteLine($"ConfigFile: {_configPath}");
       JsonText json = new JsonText(File.ReadAllText(_configPath));
       AnsiConsole.Write(
           new Panel(json)
               .Header("Some JSON in a panel")
               .Collapse()
               .RoundedBorder()
               .BorderColor(Color.Yellow));
       
       return Task.FromResult(new Unit());
   }
}
