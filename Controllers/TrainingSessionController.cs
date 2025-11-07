using System.Linq;
using System.Threading.Tasks;
using BeFit.Data;                          // ApplicationDbContext, ApplicationUser
using BeFit.Models;                        // TrainingSession (jeśli masz gdzie indziej – dostosuj przestrzeń nazw)
using Microsoft.AspNetCore.Authorization;  // [Authorize]
using Microsoft.AspNetCore.Identity;       // UserManager<T>
using Microsoft.AspNetCore.Mvc;            // Controller
using Microsoft.EntityFrameworkCore;       // EF Core

namespace BeFit.Controllers
{
  [Authorize]
  public class TrainingSessionsController : Controller
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public TrainingSessionsController(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
      _db = db; _users = users;
    }

    public async Task<IActionResult> Index()
    {
      var uid = _users.GetUserId(User);
      var list = await _db.TrainingSessions
          .AsNoTracking()
          .Where(x => x.UserId == uid)
          .OrderByDescending(x => x.StartTime)
          .ToListAsync();
      return View(list);
    }

    public async Task<IActionResult> Edit(int id)
    {
      var uid = _users.GetUserId(User);
      var entity = await _db.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity is null) return NotFound();
      return View(entity);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrainingSession model)
    {
      var uid = _users.GetUserId(User);
      if (id != model.Id) return NotFound();

      var entity = await _db.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity is null) return NotFound();

      entity.Title = model.Title;
      entity.StartTime = model.StartTime;
      entity.EndTime = model.EndTime;

      await _db.SaveChangesAsync();
      TempData["Msg"] = "Zaktualizowano sesję.";
      return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
      var uid = _users.GetUserId(User);
      var entity = await _db.TrainingSessions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity is null) return NotFound();
      return View(entity);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var uid = _users.GetUserId(User);
      var entity = await _db.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity is null) return NotFound();

      _db.TrainingSessions.Remove(entity);
      await _db.SaveChangesAsync();
      TempData["Msg"] = "Usunięto sesję.";
      return RedirectToAction(nameof(Index));
    }
  }
}
