using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreShoppingCartUI.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string userID { get; set; }

        [Required]
        public int orderStatusID { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public DateTime createDate { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
        public List<OrderDetail> orderDetails { get; set; }
    }
}