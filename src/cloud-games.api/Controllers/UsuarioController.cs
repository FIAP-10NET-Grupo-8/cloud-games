using Microsoft.AspNetCore.Mvc;

namespace cloud_games_back.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
