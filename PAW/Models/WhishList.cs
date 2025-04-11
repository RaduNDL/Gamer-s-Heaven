using System.ComponentModel.DataAnnotations;

namespace PAW.Models
{
    public class WishList
    {
        [Key]
        public int WhishListID { get; set; }

        public ICollection<WishListItem> WishListItems { get; set; } = new List<WishListItem>();
    }

}
