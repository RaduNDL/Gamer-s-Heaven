using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int GameID { get; set; }
        public  Game Game { get; set; }

        public int ShoppingCartID { get; set; }
        public  ShoppingCart ShoppingCart { get; set; }
    }

}
