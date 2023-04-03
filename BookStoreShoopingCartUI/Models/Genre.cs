using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreShoppingCartUI.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string genreName { get; set; }

        public List<Book> books { get; set; }
    }
}