using Microsoft.AspNetCore.Mvc;

namespace cloud_games_back.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
