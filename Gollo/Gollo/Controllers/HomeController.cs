using Microsoft.AspNetCore.Mvc;

namespace Gollo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Contacto")]
        public IActionResult Contacto()
        {
            return View();
        }
    }
}
