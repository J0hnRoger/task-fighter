namespace TaskFighter.Domain.DayScheduling;

public class DaySchedule 
{
    public Action<TimeBlock> OnTimeBlockEnd;
    public Action OnWorkDayEnd;
    
    private Timer _timer;
    private TimeBlock _currentBlock;

    public int TotalMinutesInDay => TimeBlocks.Sum(t => t.Interval.Duration);
    
    public int TotalMinutesPlanned => TimeBlocks.Sum(t => t.Interval.Duration);
    public List<TimeBlock> TimeBlocks { get; set; } = new ();

    public static DaySchedule CreateWorkDay()
    {
        return new DaySchedule()
        {
            TimeBlocks = new List<TimeBlock>()
            {
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(7, 0), EndTime = new Time(8, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(8, 30), EndTime = new Time(10, 00)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(10, 00), EndTime = new Time(10, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(10, 30), EndTime = new Time(12, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(14, 00), EndTime = new Time(16, 00)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(16, 00), EndTime = new Time(17, 30)}},
                new TimeBlock()
                    {Interval = new TimeInterval() {StartTime = new Time(18, 00), EndTime = new Time(20, 00)}},
            }
        };
    }
    
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