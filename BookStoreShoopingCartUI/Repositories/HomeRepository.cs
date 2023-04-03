namespace BookStoreShoppingCartUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext context;

        public HomeRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task<IEnumerable<Book>> GetBooks(string searchTerm = "", int genreID = 0)
        {
            searchTerm = searchTerm.ToLower();

            IEnumerable<Book> booksSearched = await (from book in context.Books 
                                                     join genre in context.Genres on book.genreID equals genre.Id
                                                     where string.IsNullOrWhiteSpace(searchTerm) ||
                                                     (book != null && book.bookName.ToLower().Contains(searchTerm)) //filter to search
                                                     select new Book
                                                     {
                                                         Id = book.Id,
                                                         authorName = book.authorName,
                                                         bookName = book.bookName,
                                                         genreID = book.genreID,
                                                         genreName = genre.genreName,
                                                         Price = book.Price
                                                     }).ToListAsync();

            if (genreID > 0)
                booksSearched = booksSearched.Where(b => b.genreID == genreID).ToList();

            return booksSearched;
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await context.Genres.ToListAsync();
        }
    }
}