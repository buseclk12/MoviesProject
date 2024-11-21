using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BLL.DAL;

namespace MoviesProject.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.Include(m => m.Director).ToListAsync();
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }


        // GET: Movies/Create
        public IActionResult Create()
        {
            PopulateDirectorsDropDownList();
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieCommand movieCommand)
        {
            Console.WriteLine("Formdan gelen veriler:");
            Console.WriteLine($"- Name: {movieCommand.Name}");
            Console.WriteLine($"- ReleaseDate: {movieCommand.ReleaseDate}");
            Console.WriteLine($"- TotalRevenue: {movieCommand.TotalRevenue}");
            Console.WriteLine($"- DirectorId: {movieCommand.DirectorId}");

            if (ModelState.IsValid)
            {
                // Insert item service logic:
                var result = CreateMovie(movieCommand);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = result.MovieId });
                }

                ModelState.AddModelError("", result.Message);
            }

            PopulateDirectorsDropDownList(movieCommand.DirectorId);
            return View(movieCommand);
        }

        private MovieResult CreateMovie(MovieCommand movieCommand)
        {
            try
            {
                // Check if the director exists
                if (!_context.Directors.Any(d => d.Id == movieCommand.DirectorId))
                {
                    return new MovieResult
                    {
                        IsSuccessful = false,
                        Message = "The selected Director does not exist."
                    };
                }

                var movie = new Movie
                {
                    Name = movieCommand.Name,
                    ReleaseDate = DateTime.SpecifyKind(movieCommand.ReleaseDate ?? DateTime.UtcNow, DateTimeKind.Utc), // Convert to UTC
                    TotalRevenue = movieCommand.TotalRevenue,
                    DirectorId = movieCommand.DirectorId
                };

                _context.Movies.Add(movie);
                _context.SaveChanges();

                return new MovieResult
                {
                    IsSuccessful = true,
                    Message = "Movie successfully created.",
                    MovieId = movie.Id
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating movie: {ex.Message}");
                return new MovieResult
                {
                    IsSuccessful = false,
                    Message = "An error occurred while creating the movie."
                };
            }
        }
        private void PopulateDirectorsDropDownList(object selectedDirector = null)
        {
            var directorsQuery = from d in _context.Directors
                                 orderby d.Name
                                 select d;

            ViewData["DirectorId"] = new SelectList(directorsQuery.AsNoTracking(), "Id", "Name", selectedDirector);
        }
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            var movieCommand = new MovieCommand
            {
                Name = movie.Name,
                ReleaseDate = movie.ReleaseDate,
                TotalRevenue = movie.TotalRevenue,
                DirectorId = movie.DirectorId
            };

            PopulateDirectorsDropDownList(movie.DirectorId);
            return View(movieCommand);
        }
        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieCommand movieCommand)
        {
            if (!ModelState.IsValid)
            {
                PopulateDirectorsDropDownList(movieCommand.DirectorId);
                return View(movieCommand);
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            if (!_context.Directors.Any(d => d.Id == movieCommand.DirectorId))
            {
                ModelState.AddModelError("DirectorId", "The selected Director does not exist.");
                PopulateDirectorsDropDownList(movieCommand.DirectorId);
                return View(movieCommand);
            }

            try
            {
                movie.Name = movieCommand.Name;
                movie.ReleaseDate = DateTime.SpecifyKind(movieCommand.ReleaseDate ?? DateTime.UtcNow, DateTimeKind.Utc);
                movie.TotalRevenue = movieCommand.TotalRevenue;
                movie.DirectorId = movieCommand.DirectorId;

                _context.Update(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating movie: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the movie.");
                PopulateDirectorsDropDownList(movieCommand.DirectorId);
                return View(movieCommand);
            }
        }
        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    TempData["Error"] = "Movie not found.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Movie successfully deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting movie: {ex.Message}");
                TempData["Error"] = "An error occurred while deleting the movie.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
    public class MovieCommand
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int DirectorId { get; set; }
    }

    public class MovieResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public int? MovieId { get; set; }
    }
}
