namespace BeFit.ViewModels
{
  public class StatisticsVm
  {
    public int SessionsCount { get; set; }
    public int ExercisesCount { get; set; }
    public double TotalVolume { get; set; }
    public int Last7DaysSessions { get; set; }
    public int Last30DaysSessions { get; set; }

    public List<RecentSessionVm> RecentSessions { get; set; } = new();
    public List<TopExerciseVm> TopExercises { get; set; } = new();
  }

  public class RecentSessionVm
  {
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public DateTime StartTime { get; set; }
  }

  public class TopExerciseVm
  {
    public string ExerciseTypeName { get; set; } = "";
    public int TotalReps { get; set; }
    public double TotalVolume { get; set; }
  }
}
