using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spectre.Console;

namespace TaskSamurai.Infrastructure;

public record ShowConfigRequest : IRequest<Unit>
{
    public ShowConfigRequest(string value, string filters)
    {
        
    }

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
       return Task.FromResult(new Unit());
   }
}
