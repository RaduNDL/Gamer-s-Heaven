using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class WishListItem
    {
        [Key]
        public int WishListItemID { get; set; }

        public int GameID { get; set; }
        public Game Game { get; set; }

        public string? GameTitle { get; set; }

        public int WhishListID { get; set; }
        public WishList WishList { get; set; }
    }
}
