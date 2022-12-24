namespace TaskSamurai.Domain.DayScheduling;

public class DaySchedule 
{
    public Action<TimeBlock> OnTimeBlockEnd;
    public Action OnWorkDayEnd;
    
    private Timer _timer;
    private TimeBlock _currentBlock;

    public int TotalMinutesPlanned => TimeBlocks.Sum(t => t.Interval.Duration);
    public List<TimeBlock> TimeBlocks { get; set; } = new ();

    public void StartDay(DateTime now)
    {
        if (TimeBlocks.Count == 0)
            throw new Exception("Aucun TimeBlock définis pour cette journée");
        _currentBlock = TimeBlocks.First(t => t.Interval.Include(now));
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
    }

    private void TimerCallback(object? state)
    {
        if (_currentBlock.IsDone(DateTime.Now))
        {
            OnTimeBlockEnd?.Invoke(_currentBlock);    
            if (TimeBlocks.Count == 0)
            {
                _timer.Change(Timeout.Infinite, 0);
                OnWorkDayEnd?.Invoke();
            }
        }
    }
}