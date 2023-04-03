using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreShoppingCartUI.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }

        [Required]
        public int statusID { get; set; }

        [Required, MaxLength(20)]
        public string? statusName { get; set; }
    }
}