using System;
using System.Linq;
using System.Threading.Tasks;
using BeFit.Data;                    
using BeFit.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
  [Authorize]
  public class StatsController : Controller
  {
    private readonly ApplicationDbContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;

    public StatsController(ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
    {
      _ctx = ctx;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      var uid = _userManager.GetUserId(User);
      var since = DateTime.UtcNow.Date.AddDays(-28);

      var data = await _ctx.PerformedExercises
          .AsNoTracking()
          .Include(pe => pe.TrainingSession)
          .Include(pe => pe.ExerciseType)
          .Where(pe => pe.TrainingSession != null &&
                       pe.TrainingSession.UserId == uid &&
                       pe.TrainingSession.StartTime >= since)
          .GroupBy(pe => pe.ExerciseType!.Name)
          .Select(g => new StatsVm
          {
            ExerciseTypeName = g.Key,
            SessionsCount = g.Select(x => x.TrainingSessionId).Distinct().Count(),
            TotalSets = g.Sum(x => x.Sets),
            TotalReps = g.Sum(x => x.Sets * x.Reps),
            TotalWeight = g.Sum(x => (x.Weight ?? 0) * x.Sets * x.Reps)
          })
          .OrderByDescending(s => s.TotalReps)
          .ToListAsync();

      return View(data);
    }
  }
}
