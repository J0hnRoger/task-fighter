using TaskSamurai.Domain.Common;

namespace TaskSamurai.Domain.DayReporting;

/// <summary>
/// Chaque jour, noter les métriques importantes qui permettent d'évaluer la
/// productivité de la journée
/// ET mon état physique / mental pour détecter des ajustements à faire 
/// </summary>
public class DailyReport : Entity
{
   public int EnergyLevel { get; set; } 
   public int ConcentrationLevel { get; set; } 
   public int MoodLevel { get; set; } 
   public string Notes { get; set; }
   public int FocusedDuration { get; set; }
}