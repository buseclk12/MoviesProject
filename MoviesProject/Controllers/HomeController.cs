using Microsoft.AspNetCore.Mvc;

namespace MoviesProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}