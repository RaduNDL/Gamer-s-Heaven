using Microsoft.AspNetCore.Mvc;
using PAW.Data;
using Microsoft.EntityFrameworkCore;

namespace PAW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topGames = await _context.Games
                .OrderByDescending(g => g.ReleaseDate)
                .Take(3)
                .ToListAsync();

            return View(topGames);
        }
    }
}
