namespace BeFit.Models
{
  public class StatsVm
  {
    public string ExerciseTypeName { get; set; } = "";
    public int SessionsCount { get; set; }
    public int TotalSets { get; set; }
    public int TotalReps { get; set; }
    public double TotalWeight { get; set; }
  }
}
