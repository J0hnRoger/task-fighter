namespace TaskSamurai.Domain.DayReporting;

/// <summary>
/// Chaque jour, noter les métriques importantes qui permettent d'évaluer la
/// productivité de la journée
/// ET mon état physique / mental pour détecter des ajustements à faire 
/// </summary>
public class DailyReport
{
   public int RemainingEnergyLevel { get; set; } 
   public int ConcentrationLevel { get; set; } 
   public int MoodLevel { get; set; } 
}