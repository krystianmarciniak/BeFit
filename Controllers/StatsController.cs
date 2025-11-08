using System;
using System.Linq;
using System.Threading.Tasks;
using BeFit.Data;
using BeFit.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
  [Authorize(Roles = "Admin")]
  public class StatsController : Controller
  {
    private readonly ApplicationDbContext _ctx;
    private readonly UserManager<ApplicationUser> _users;

    public StatsController(ApplicationDbContext ctx, UserManager<ApplicationUser> users)
    {
      _ctx = ctx;
      _users = users;
    }

    // Globalne podsumowanie dla Admina z ostatnich 28 dni
    public async Task<IActionResult> Index()
    {
      var since = DateTime.UtcNow.Date.AddDays(-28);

      var data = await _ctx.PerformedExercises
          .AsNoTracking()
          .Include(pe => pe.TrainingSession)
          .Include(pe => pe.ExerciseType)
          .Where(pe => pe.TrainingSession != null &&
                       pe.TrainingSession.StartTime >= since)
          .GroupBy(pe => pe.ExerciseType!.Name)
          .Select(g => new StatsVm
          {
            ExerciseTypeName = g.Key,
            SessionsCount = g.Select(x => x.TrainingSessionId).Distinct().Count(),
            TotalSets = g.Sum(x => x.Sets),
            TotalReps = g.Sum(x => x.Sets * x.Reps),
            TotalWeight = g.Sum(x => (x.WeightKg ?? 0) * x.Sets * x.Reps)
          })
          .OrderByDescending(x => x.TotalReps)
          .ToListAsync();

      return View(data);
    }
  }
}
