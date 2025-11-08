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
  [Authorize]
  public class StatisticsController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public StatisticsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      var uid = _userManager.GetUserId(User);
      var now = DateTime.UtcNow;

      var sessionsQ = _context.TrainingSessions.Where(s => s.UserId == uid);

      var exercisesQ = _context.PerformedExercises
          .Include(x => x.TrainingSession)
          .Include(x => x.ExerciseType)
          .Where(x => x.TrainingSession != null && x.TrainingSession.UserId == uid);

      var vm = new StatisticsVm
      {
        SessionsCount = await sessionsQ.CountAsync(),
        ExercisesCount = await exercisesQ.CountAsync(),
        TotalVolume = await exercisesQ
                                  .Select(x => (double)(x.Sets * x.Reps) * (x.WeightKg ?? 0))
                                  .SumAsync(),
        Last7DaysSessions = await sessionsQ.CountAsync(s => s.StartTime >= now.AddDays(-7)),
        Last30DaysSessions = await sessionsQ.CountAsync(s => s.StartTime >= now.AddDays(-30)),
      };

      // Ostatnie 5 sesji
      vm.RecentSessions = await sessionsQ.AsNoTracking()
          .OrderByDescending(s => s.StartTime)
          .Take(5)
          .Select(s => new RecentSessionVm
          {
            Id = s.Id,
            Title = s.Title,
            StartTime = (s.StartTime ?? s.SessionDate).Date
          })
          .ToListAsync();

      // Top 5 ćwiczeń wg sumy powtórzeń
      vm.TopExercises = await exercisesQ.AsNoTracking()
          .GroupBy(x => x.ExerciseType!.Name)
          .Select(g => new TopExerciseVm
          {
            ExerciseTypeName = g.Key,
            TotalReps = g.Sum(x => x.Sets * x.Reps),
            TotalVolume = g.Sum(x => (x.WeightKg ?? 0) * x.Sets * x.Reps)
          })
          .OrderByDescending(x => x.TotalReps)
          .Take(5)
          .ToListAsync();

      return View(vm);
    }
  }
}
