using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Cart
        public async Task<IActionResult> Index()
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new ShoppingCart { CartItems = new List<CartItem>() };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null || request.GameId == 0)
                return BadRequest("Invalid game ID.");

            var game = await _context.Games.FindAsync(request.GameId);
            if (game == null)
                return NotFound("Game not found.");

            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                cart = new ShoppingCart { CartItems = new List<CartItem>() };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.GameID == request.GameId);
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    GameID = game.GameID,
                    Quantity = 1,
                    Price = game.Price,
                    ShoppingCartID = cart.CartID
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Game added to cart!" });
        }

        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var item = await _context.CartItems
                .Include(ci => ci.ShoppingCart)
                .FirstOrDefaultAsync(ci => ci.CartItemID == cartItemId);

            if (item != null)
            {
                var cart = item.ShoppingCart;
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();

                var itemsLeft = await _context.CartItems
                    .Where(ci => ci.ShoppingCartID == cart.CartID)
                    .ToListAsync();

                if (!itemsLeft.Any())
                {
                    _context.ShoppingCarts.Remove(cart);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, string action)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                if (action == "increase")
                    item.Quantity += 1;
                else if (action == "decrease" && item.Quantity > 1)
                    item.Quantity -= 1;
                else if (action == "decrease")
                    _context.CartItems.Remove(item);

                await _context.SaveChangesAsync();

                var cart = await _context.ShoppingCarts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.CartID == item.ShoppingCartID);

                if (cart != null && !cart.CartItems.Any())
                {
                    _context.ShoppingCarts.Remove(cart);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        // GET: /Cart/Billing
        [HttpGet]
        public async Task<IActionResult> Billing()
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync();

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Message"] = "Cart is empty.";
                return RedirectToAction("Index");
            }

            return View("Billing", cart);
        }

        // POST: /Cart/ProcessPayment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string cardNumber, string nameOnCard, string expiryDate, string cvv)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || string.IsNullOrWhiteSpace(nameOnCard))
            {
                TempData["Message"] = "Payment failed. Please fill out all required fields.";
                return RedirectToAction("Billing");
            }

            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                _context.ShoppingCarts.Remove(cart);
                await _context.SaveChangesAsync();
            }

            TempData["Message"] = "✅ Payment successful! Thank you for your purchase.";
            return RedirectToAction("Index");
        }

        // GET: /Cart/GetItemCount
        [HttpGet]
        public async Task<IActionResult> GetItemCount()
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            int count = cart?.CartItems?.Sum(i => i.Quantity) ?? 0;
            return Json(new { count });
        }

        public class AddToCartRequest
        {
            public int GameId { get; set; }
        }
    }
}
