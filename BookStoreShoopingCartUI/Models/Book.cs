using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreShoppingCartUI.Models
{
    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? bookName { get; set; }

        [MaxLength(40)]
        public string? authorName { get; set; }

        public string? imageCover { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int genreID { get; set; }

        public Genre Genre { get; set; }
        public List<OrderDetail> orderDetail { get; set; }
        public List<CartDetail> cartDetail { get; set; }

        [NotMapped] //not in DB
        public string genreName { get; set; }
    }
}