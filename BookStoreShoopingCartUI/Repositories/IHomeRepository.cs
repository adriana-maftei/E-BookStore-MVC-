namespace BookStoreShoppingCartUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBooks(string searchTerm = "", int genreID = 0);
        Task<IEnumerable<Genre>> GetGenres();
    }
}