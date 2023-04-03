namespace BookStoreShoppingCartUI.Models.DTOs
{
    public class BookDisplayModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public string searchTerm { get; set; } = "";
        public int genreID { get; set; } = 0;
    }
}
