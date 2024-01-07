using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using TaskFighter.Domain.DayScheduling;

namespace TaskFighter.Infrastructure.Persistence;

public class DayScheduleRepository
{
    private readonly string _daySchedulePath;
    private readonly string _dayScheduleDirectory;
    private DaySchedule _dayTemplate;

    public DayScheduleRepository(string dayScheduleTemplatePath, string dayScheduleDirectory)
    {
        _daySchedulePath = dayScheduleTemplatePath;
        _dayScheduleDirectory = dayScheduleDirectory.TrimEnd('/') + "/";

        string content = File.ReadAllText(_daySchedulePath);
        _dayTemplate = JsonConvert.DeserializeObject<DaySchedule>(content);
    }

    public DaySchedule GetTemplate()
    {
        return _dayTemplate;
    }
    
    public Result<DaySchedule> GetCurrent()
    {
        string today = DateTime.Today.ToString("dd-MM-yyyy");
        return Get(today);
    }
    
    /// <summary>
    /// Return DaySchedule at the date specified   
    /// </summary>
    /// <param name="dateFormat">dd-MM-yyyy eg: 24-12-2022</param>
    /// <returns></returns>
    public Result<DaySchedule> Get(string dateFormat)
    {
        string daySchedulePath = _dayScheduleDirectory + dateFormat + ".json";
        if (!File.Exists(daySchedulePath))
            return Result.Failure<DaySchedule>("File doesn't exist yet");

        string content = File.ReadAllText(daySchedulePath);
        DaySchedule current = JsonConvert.DeserializeObject<DaySchedule>(content);
        
        return Result.Success(current);
    }

    public void Create(string date, DaySchedule currentDay)
    {
        File.WriteAllText(_dayScheduleDirectory + date + ".json",JsonConvert.SerializeObject(currentDay));
    }
    
    public void UpdateTemplate(DaySchedule daySchedule)
    {
        File.WriteAllText(_daySchedulePath,JsonConvert.SerializeObject(daySchedule));
    }
}