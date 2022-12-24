using MediatR;

namespace TaskSamurai.Domain.DayScheduling.Commands;

public record GenerateDayScheduleCommand : IRequest<DaySchedule> 
{
   public DateTime CurrentDay { get; set; } 
}

public class GenerateDayScheduleCommandHandler : IRequestHandler<GenerateDayScheduleCommand, DaySchedule>
{
   public Task<DaySchedule> Handle(GenerateDayScheduleCommand request, CancellationToken cancellationToken)
   {
     return Task.FromResult(new DaySchedule()); 
   }
}

