using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class Game
    {
        [Key]
        public int GameID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string ImageFileName { get; set; } 
        public ICollection<Review> Reviews { get; set; }    
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<WishListItem> WishListItems { get; set; }
    }

}
