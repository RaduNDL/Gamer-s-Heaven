using Microsoft.AspNetCore.Mvc;

namespace PAW.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
