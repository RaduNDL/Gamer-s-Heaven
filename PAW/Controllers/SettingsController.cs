using Microsoft.AspNetCore.Mvc;

namespace PAW.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
