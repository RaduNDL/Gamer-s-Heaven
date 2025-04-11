using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        public required ICollection<CartItem> CartItems { get; set; }
    }

}
