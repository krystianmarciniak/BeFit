using BeFit.Models;
namespace BeFit.ViewModels
{
  public class PerformedExerciseIndexVm
  {
    public IReadOnlyList<PerformedExercise> Items { get; set; } = Array.Empty<PerformedExercise>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / Math.Max(1, PageSize));
  }
}
