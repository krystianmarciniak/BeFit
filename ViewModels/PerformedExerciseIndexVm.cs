using BeFit.Models;
namespace BeFit.ViewModels
{
  public class PerformedExerciseIndexVm
  {
    public IEnumerable<BeFit.Models.PerformedExercise> Items { get; set; } = Enumerable.Empty<BeFit.Models.PerformedExercise>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / Math.Max(1, PageSize));
  }
}
