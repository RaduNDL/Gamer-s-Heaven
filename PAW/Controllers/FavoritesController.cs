using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var wishlist = await _context.WishLists
                .Include(w => w.WishListItems)
                .ThenInclude(wli => wli.Game)
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                wishlist = new WishList();
                _context.WishLists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistRequest request)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameID == request.GameId);
            if (game == null) return NotFound();

            var wishlist = await _context.WishLists
                .Include(w => w.WishListItems)
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                wishlist = new WishList();
                _context.WishLists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            if (!wishlist.WishListItems.Any(i => i.GameID == game.GameID))
            {
                var newItem = new WishListItem
                {
                    GameID = game.GameID,
                    GameTitle = game.Title,
                    WhishListID = wishlist.WhishListID
                };

                _context.WishListItems.Add(newItem);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Game added to wishlist!" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWishlist(int wishListItemId)
        {
            var item = await _context.WishListItems.FindAsync(wishListItemId);

            if (item != null)
            {
                var wishlistId = item.WhishListID;

                _context.WishListItems.Remove(item);
                await _context.SaveChangesAsync();

                var remaining = await _context.WishListItems
                    .Where(w => w.WhishListID == wishlistId)
                    .ToListAsync();

                if (!remaining.Any())
                {
                    var wishlist = await _context.WishLists.FindAsync(wishlistId);
                    if (wishlist != null)
                    {
                        _context.WishLists.Remove(wishlist);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public class AddToWishlistRequest
        {
            public int GameId { get; set; }
        }
    }
}
