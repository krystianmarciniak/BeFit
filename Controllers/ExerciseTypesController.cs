// Controllers/ExerciseTypesController.cs
using BeFit.Models; // <<< ważne
using BeFit.Data;
//using BeFit.Features.Exercises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExerciseTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseTypesController(ApplicationDbContext context) => _context = context;

        // GET: /ExerciseTypes
        public async Task<IActionResult> Index(string? q)
        {
            ViewBag.Query = q;
            var query = _context.ExerciseTypes.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(x => x.Name.Contains(q));
            var list = await query.OrderBy(x => x.Name).ToListAsync();
            return View(list);
        }

        // GET: /ExerciseTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.ExerciseTypes.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: /ExerciseTypes/Create
        public IActionResult Create() => View(new ExerciseType());

        // POST: /ExerciseTypes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseType model)
        {
            if (await _context.ExerciseTypes.AnyAsync(x => x.Name == model.Name))
                ModelState.AddModelError(nameof(model.Name), "Taka nazwa już istnieje.");

            if (!ModelState.IsValid) return View(model);

            _context.Add(model);
            await _context.SaveChangesAsync();
            TempData["ok"] = "Dodano nowy rodzaj ćwiczenia.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /ExerciseTypes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.ExerciseTypes.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /ExerciseTypes/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExerciseType model)
        {
            if (id != model.Id) return BadRequest();

            if (await _context.ExerciseTypes
                    .AnyAsync(x => x.Id != id && x.Name == model.Name))
                ModelState.AddModelError(nameof(model.Name), "Taka nazwa już istnieje.");

            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["ok"] = "Zapisano zmiany.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ExerciseTypes.AnyAsync(x => x.Id == id))
                    return NotFound();
                throw;
            }
        }

        // GET: /ExerciseTypes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ExerciseTypes.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: /ExerciseTypes/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.ExerciseTypes.FindAsync(id);
            if (item == null) return NotFound();

            _context.Remove(item);
            await _context.SaveChangesAsync();
            TempData["ok"] = "Usunięto.";
            return RedirectToAction(nameof(Index));
        }
    }
}
