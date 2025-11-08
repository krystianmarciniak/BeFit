using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BeFit.Data;
using BeFit.Models;

namespace BeFit.Controllers
{
  [Authorize]
  public class TrainingSessionsController : Controller
  {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TrainingSessionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    // GET: /TrainingSessions
    public async Task<IActionResult> Index()
    {
      var uid = _userManager.GetUserId(User);
      var data = await _context.TrainingSessions
          .Where(x => x.UserId == uid)
          .OrderByDescending(x => x.SessionDate)
          .ToListAsync();

      return View(data);
    }

    // GET: /TrainingSessions/Create
    public IActionResult Create() => View();

    // POST: /TrainingSessions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainingSession model)
    {
      var uid = _userManager.GetUserId(User);

      // Jeżeli tytuł jest pusty, ustaw domyślny i wyczyść błąd walidacji
      if (string.IsNullOrWhiteSpace(model.Title))
      {
        model.Title = model.SessionDate.ToString("yyyy-MM-dd HH:mm");
        ModelState.Remove(nameof(model.Title));
      }

      if (!ModelState.IsValid)
        return View(model);

      model.UserId = uid;
      _context.TrainingSessions.Add(model);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }


    // GET: /TrainingSessions/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
      var uid = _userManager.GetUserId(User);
      var entity = await _context.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity == null) return NotFound();
      return View(entity);
    }

    // POST: /TrainingSessions/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TrainingSession model)
    {
      var uid = _userManager.GetUserId(User);
      var entity = await _context.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity == null) return NotFound();

      if (!ModelState.IsValid) return View(model);

      entity.Date = model.Date;
      entity.DurationMinutes = model.DurationMinutes;
      entity.Notes = model.Notes;

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    // GET: /TrainingSessions/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
      var uid = _userManager.GetUserId(User);
      var entity = await _context.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity == null) return NotFound();
      return View(entity);
    }

    // POST: /TrainingSessions/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var uid = _userManager.GetUserId(User);
      var entity = await _context.TrainingSessions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
      if (entity == null) return NotFound();

      _context.TrainingSessions.Remove(entity);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }
  }
}
