using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreShoppingCartUI.Models
{
    [Table("ShopppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public string userID { get; set; }

        public bool isDeleted { get; set; } = false;

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}