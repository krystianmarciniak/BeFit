using System.Linq;
using System.Threading.Tasks;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
  [Authorize]
  public class PerformedExerciseController : Controller
  {
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public PerformedExerciseController(ApplicationDbContext db, UserManager<ApplicationUser> users)
    {
      _db = db;
      _users = users;
    }

    // GET: /PerformedExercise
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
      page = Math.Max(1, page);
      pageSize = Math.Clamp(pageSize, 5, 50);

      var uid = _users.GetUserId(User);

      // bazowe zapytanie: tylko rekordy użytkownika i tylko te z przypisaną sesją
      var baseQ = _db.PerformedExercises
          .AsNoTracking()
          .Include(x => x.ExerciseType)
          .Include(x => x.TrainingSession)
          .Where(x => x.TrainingSession != null && x.TrainingSession.UserId == uid);

      var total = await baseQ.CountAsync();

      // Sortowanie:
      // StartTime jest nullable, SessionDate nie — używamy koalescencji,
      // co tłumaczy się w SQLite na COALESCE i działa w SQL (nie po stronie klienta).
      var items = await baseQ
          .OrderByDescending(x => (x.TrainingSession!.StartTime ?? x.TrainingSession!.SessionDate))
          .ThenBy(x => x.ExerciseType!.Name)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      var vm = new BeFit.ViewModels.PerformedExerciseIndexVm
      {
        Items = items,
        Page = page,
        PageSize = pageSize,
        TotalCount = total
      };

      return View(vm);
    }

    // GET: /PerformedExercise/Details/5
    public async Task<IActionResult> Details(int id)
    {
      var uid = _users.GetUserId(User);

      var entity = await _db.PerformedExercises
          .AsNoTracking()
          .Include(x => x.ExerciseType)
          .Include(x => x.TrainingSession)
          .FirstOrDefaultAsync(x => x.Id == id &&
                                    x.TrainingSession != null &&
                                    x.TrainingSession.UserId == uid);

      if (entity is null) return NotFound();
      return View(entity);
    }

    // GET: /PerformedExercise/Create
    public async Task<IActionResult> Create()
    {
      await FillSelectListsAsync();
      return View(new PerformedExercise());
    }

    // POST: /PerformedExercise/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PerformedExercise model)
    {
      var uid = _users.GetUserId(User);

      var ownsSession = await _db.TrainingSessions
          .AnyAsync(s => s.Id == model.TrainingSessionId && s.UserId == uid);
      if (!ownsSession)
        ModelState.AddModelError(nameof(model.TrainingSessionId), "Nie możesz dodać ćwiczenia do cudzej sesji.");

      var exerciseTypeExists = await _db.ExerciseTypes.AnyAsync(e => e.Id == model.ExerciseTypeId);
      if (!exerciseTypeExists)
        ModelState.AddModelError(nameof(model.ExerciseTypeId), "Wybrany typ ćwiczenia nie istnieje.");

      if (model.Sets <= 0) ModelState.AddModelError(nameof(model.Sets), "Liczba serii musi być > 0.");
      if (model.Reps <= 0) ModelState.AddModelError(nameof(model.Reps), "Liczba powtórzeń musi być > 0.");
      if (model.WeightKg < 0) ModelState.AddModelError(nameof(model.WeightKg), "Ciężar nie może być ujemny.");

      if (!ModelState.IsValid)
      {
        await FillSelectListsAsync(uid, model.TrainingSessionId);
        return View(model);
      }

      _db.PerformedExercises.Add(model);
      await _db.SaveChangesAsync();

      TempData["Msg"] = "Dodano wykonane ćwiczenie.";
      return RedirectToAction(nameof(Index));
    }

    // GET: /PerformedExercise/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
      var uid = _users.GetUserId(User);

      var entity = await _db.PerformedExercises
          .Include(x => x.TrainingSession)
          .FirstOrDefaultAsync(x => x.Id == id &&
                                    x.TrainingSession != null &&
                                    x.TrainingSession.UserId == uid);

      if (entity is null) return NotFound();

      await FillSelectListsAsync(uid, entity.TrainingSessionId);
      return View(entity);
    }

    // POST: /PerformedExercise/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PerformedExercise model)
    {
      if (id != model.Id) return NotFound();

      var uid = _users.GetUserId(User);

      var entity = await _db.PerformedExercises
          .Include(x => x.TrainingSession)
          .FirstOrDefaultAsync(x => x.Id == id &&
                                    x.TrainingSession != null &&
                                    x.TrainingSession.UserId == uid);

      if (entity is null) return NotFound();

      var ownsTargetSession = await _db.TrainingSessions
          .AnyAsync(s => s.Id == model.TrainingSessionId && s.UserId == uid);
      if (!ownsTargetSession)
        ModelState.AddModelError(nameof(model.TrainingSessionId), "Nie możesz przenieść ćwiczenia do cudzej sesji.");

      var exerciseTypeExists = await _db.ExerciseTypes.AnyAsync(e => e.Id == model.ExerciseTypeId);
      if (!exerciseTypeExists)
        ModelState.AddModelError(nameof(model.ExerciseTypeId), "Wybrany typ ćwiczenia nie istnieje.");

      if (model.Sets <= 0) ModelState.AddModelError(nameof(model.Sets), "Liczba serii musi być > 0.");
      if (model.Reps <= 0) ModelState.AddModelError(nameof(model.Reps), "Liczba powtórzeń musi być > 0.");
      if (model.WeightKg < 0) ModelState.AddModelError(nameof(model.WeightKg), "Ciężar nie może być ujemny.");

      if (!ModelState.IsValid)
      {
        await FillSelectListsAsync(uid, model.TrainingSessionId);
        return View(model);
      }

      entity.ExerciseTypeId = model.ExerciseTypeId;
      entity.TrainingSessionId = model.TrainingSessionId;
      entity.Sets = model.Sets;
      entity.Reps = model.Reps;
      entity.WeightKg = model.WeightKg;

      await _db.SaveChangesAsync();
      TempData["Msg"] = "Zaktualizowano ćwiczenie.";
      return RedirectToAction(nameof(Index));
    }

    // GET: /PerformedExercise/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
      var uid = _users.GetUserId(User);

      var entity = await _db.PerformedExercises
          .AsNoTracking()
          .Include(x => x.ExerciseType)
          .Include(x => x.TrainingSession)
          .FirstOrDefaultAsync(x => x.Id == id &&
                                    x.TrainingSession != null &&
                                    x.TrainingSession.UserId == uid);

      if (entity is null) return NotFound();
      return View(entity);
    }

    // POST: /PerformedExercise/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var uid = _users.GetUserId(User);

      var entity = await _db.PerformedExercises
          .Include(x => x.TrainingSession)
          .FirstOrDefaultAsync(x => x.Id == id &&
                                    x.TrainingSession != null &&
                                    x.TrainingSession.UserId == uid);

      if (entity is null) return NotFound();

      _db.PerformedExercises.Remove(entity);
      await _db.SaveChangesAsync();

      TempData["Msg"] = "Usunięto ćwiczenie.";
      return RedirectToAction(nameof(Index));
    }

    // ---- Pomocnicze ----
    private async Task FillSelectListsAsync(string? uid = null, int? selectedSessionId = null)
    {
      uid ??= _users.GetUserId(User);

      var exerciseTypes = await _db.ExerciseTypes
          .AsNoTracking()
          .OrderBy(e => e.Name)
          .Select(e => new { e.Id, e.Name })
          .ToListAsync();

      var sessions = await _db.TrainingSessions
          .AsNoTracking()
          .Where(s => s.UserId == uid)
          .OrderByDescending(s => s.StartTime)
          .Select(s => new { s.Id, s.Title })
          .ToListAsync();

      ViewBag.ExerciseTypeId = new SelectList(exerciseTypes, "Id", "Name");
      ViewBag.TrainingSessionId = new SelectList(sessions, "Id", "Title", selectedSessionId);
    }
  }
}
