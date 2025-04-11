using Microsoft.AspNetCore.Mvc;

namespace PAW.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
