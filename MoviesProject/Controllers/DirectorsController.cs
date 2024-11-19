using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.DAL;

namespace MoviesProject.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly AppDbContext _context;

        public DirectorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Directors.ToListAsync());
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,IsRetired")] Director director)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(director);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving director: {ex.Message}");
                }
            }

            return View(director);
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var director = await _context.Directors.FindAsync(id);
            if (director == null) return NotFound();

            return View(director);
        }

        // POST: Directors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,IsRetired")] Director director)
        {
            if (id != director.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating director: {ex.Message}");
                }
            }

            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var director = await _context.Directors
                .FirstOrDefaultAsync(d => d.Id == id);

            if (director == null) return NotFound();

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var director = await _context.Directors.FindAsync(id);
                if (director == null) 
                {
                    TempData["Error"] = "Director not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Director successfully deleted.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting director: {ex.Message}");
                TempData["Error"] = "An error occurred while deleting the director.";
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var director = await _context.Directors
                .FirstOrDefaultAsync(d => d.Id == id);

            if (director == null) return NotFound();

            return View(director);
        }
    }
}
