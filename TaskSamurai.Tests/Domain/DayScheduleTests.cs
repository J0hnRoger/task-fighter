using System.Threading;
using System.Threading.Tasks;
using TaskSamurai.Domain.DayScheduling.Requests;
using TaskSamurai.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class DayRequestTests
{
    [Fact]
    public async Task EndDayRequest_CreateReporting()
    {
        SamuraiTasksContext samuraiTasksContext = TestHelpers.CreateTestContext();
        var handler = new EndDayScheduleRequestHandler(samuraiTasksContext);
        var result = await handler.Handle(new EndDayScheduleRequest("test des notes energy:3", "")
            , CancellationToken.None);
        
    }
}