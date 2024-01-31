using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TaskFighter.Domain.DayScheduling.Requests;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class DayRequestTests
{
    [Fact]
    public async Task StartDayScheduleRequest_CreateReporting()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new StartDayScheduleRequestHandler(fighterTasksContext);
        var result = await handler.Handle(new StartDayScheduleRequest("test des notes energy:3", new Filters(""))
            , CancellationToken.None);
    }
    
    [Fact]
    public async Task EndDayRequest_CreateReporting()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new EndDayRequestHandler(fighterTasksContext);
        var result = await handler.Handle(new EndDayRequest("test des notes energy:3", new Filters(""))
            , CancellationToken.None);
         
    }
}