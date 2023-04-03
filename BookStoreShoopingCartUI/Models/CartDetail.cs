using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreShoppingCartUI.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }

        [Required]
        public int shoppingCartID { get; set; }

        [Required]
        public int bookID { get; set; }

        public ShoppingCart ShopppingCart { get; set; }
        public Book Book { get; set; }

        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
    }
}