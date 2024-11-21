using Microsoft.AspNetCore.Mvc;
using BLL.DAL;

namespace MoviesProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.RoleId = 2;
                user.IsActive = true;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }

            return View(user);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(
                u => u.UserName == username && u.Password == password && u.IsActive);
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return RedirectToAction("Index", "Movies");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}