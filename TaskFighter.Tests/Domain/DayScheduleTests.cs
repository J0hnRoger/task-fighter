using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TaskFighter.Domain.DayScheduling.Requests;
using TaskFighter.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class DayRequestTests
{
    [Fact]
    public async Task PlanDayScheduleRequest_CreateReporting()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new PlanDayScheduleRequestHandler(fighterTasksContext);
        var result = await handler.Handle(new PlanDayScheduleRequest("", "")
            , CancellationToken.None);
        
        result.TimeBlocks.Count.Should().Be(7);
        result.TotalMinutesPlanned.Should().Be(0);
    }
    
    [Fact]
    public async Task StartDayScheduleRequest_CreateReporting()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new StartDayScheduleRequestHandler(fighterTasksContext);
        var result = await handler.Handle(new StartDayScheduleRequest("test des notes energy:3", "")
            , CancellationToken.None);
    }
    
    [Fact]
    public async Task EndDayRequest_CreateReporting()
    {
        FighterTasksContext fighterTasksContext = TestHelpers.CreateTestContext();
        var handler = new EndDayRequestHandler(fighterTasksContext);
        var result = await handler.Handle(new EndDayRequest("test des notes energy:3", "")
            , CancellationToken.None);
         
    }
}