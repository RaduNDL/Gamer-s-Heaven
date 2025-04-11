using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Controllers
{
    [Route("reviews")]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetReviews(int gameId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.GameID == gameId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return Json(reviews);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            review.Date = DateTime.Now;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }
        [HttpGet("/Reviews")]
        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return View(reviews);
        }
    }

}
